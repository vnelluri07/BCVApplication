using System.Text.Json;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.UI.Services;
using Microsoft.JSInterop;
using Moq;

namespace BeersCheersVasis.Tests.Services;

public class OfflineSyncServiceTests
{
    private readonly Mock<IJSRuntime> _mockJs;
    private readonly Mock<IScriptApi> _mockScriptApi;
    private readonly OfflineSyncService _sut;

    public OfflineSyncServiceTests()
    {
        _mockJs = new Mock<IJSRuntime>();
        _mockScriptApi = new Mock<IScriptApi>();
        _sut = new OfflineSyncService(_mockJs.Object, _mockScriptApi.Object);
    }

    // --- IsOnlineAsync ---

    [Fact]
    public async Task IsOnlineAsync_ReturnsTrue_WhenBrowserOnline()
    {
        _mockJs.Setup(j => j.InvokeAsync<bool>("bcvOffline.isOnline", It.IsAny<object[]>()))
            .ReturnsAsync(true);

        Assert.True(await _sut.IsOnlineAsync());
    }

    [Fact]
    public async Task IsOnlineAsync_ReturnsFalse_WhenBrowserOffline()
    {
        _mockJs.Setup(j => j.InvokeAsync<bool>("bcvOffline.isOnline", It.IsAny<object[]>()))
            .ReturnsAsync(false);

        Assert.False(await _sut.IsOnlineAsync());
    }

    // --- EnqueueDraftAsync ---

    [Fact]
    public async Task EnqueueDraftAsync_SavesDraftToLocalStorage()
    {
        string? savedJson = null;
        SetupEmptyQueue();
        _mockJs.Setup(j => j.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "bcvOffline.saveDrafts", It.IsAny<object[]>()))
            .Callback<string, object[]>((_, args) => savedJson = args[0] as string)
            .ReturnsAsync(Mock.Of<Microsoft.JSInterop.Infrastructure.IJSVoidResult>());

        var draft = new OfflineDraft { LocalId = "abc", Title = "Test", Content = "<p>hi</p>", UserId = 22 };
        await _sut.EnqueueDraftAsync(draft);

        Assert.NotNull(savedJson);
        var drafts = JsonSerializer.Deserialize<List<OfflineDraft>>(savedJson!);
        Assert.Single(drafts!);
        Assert.Equal("Test", drafts![0].Title);
    }

    [Fact]
    public async Task EnqueueDraftAsync_UpdatesExistingDraft_WhenSameLocalId()
    {
        var existing = new OfflineDraft { LocalId = "abc", Title = "Old" };
        SetupQueueWith([existing]);

        string? savedJson = null;
        _mockJs.Setup(j => j.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "bcvOffline.saveDrafts", It.IsAny<object[]>()))
            .Callback<string, object[]>((_, args) => savedJson = args[0] as string)
            .ReturnsAsync(Mock.Of<Microsoft.JSInterop.Infrastructure.IJSVoidResult>());

        await _sut.EnqueueDraftAsync(new OfflineDraft { LocalId = "abc", Title = "Updated" });

        var drafts = JsonSerializer.Deserialize<List<OfflineDraft>>(savedJson!);
        Assert.Single(drafts!);
        Assert.Equal("Updated", drafts![0].Title);
    }

    // --- SyncAllAsync ---

    [Fact]
    public async Task SyncAllAsync_ReturnsZero_WhenQueueEmpty()
    {
        SetupEmptyQueue();

        var result = await _sut.SyncAllAsync();

        Assert.Equal(0, result.SyncedCount);
        Assert.Null(result.LastScriptId);
    }

    [Fact]
    public async Task SyncAllAsync_CreatesNewScript_WhenNoScriptId()
    {
        var draft = new OfflineDraft { LocalId = "a", Title = "New", Content = "body", UserId = 22 };
        SetupQueueWith([draft]);
        SetupClearDrafts();

        _mockScriptApi.Setup(a => a.CreateAsync(It.IsAny<CreateScriptRequest>()))
            .ReturnsAsync(new ScriptResponse { Id = 99, Title = "New" });

        var result = await _sut.SyncAllAsync();

        Assert.Equal(1, result.SyncedCount);
        Assert.Equal(99, result.LastScriptId);
        _mockScriptApi.Verify(a => a.CreateAsync(It.Is<CreateScriptRequest>(r => r.Title == "New")), Times.Once);
    }

    [Fact]
    public async Task SyncAllAsync_UpdatesExistingScript_WhenHasScriptId()
    {
        var draft = new OfflineDraft { LocalId = "a", ScriptId = 42, Title = "Edit", Content = "body", UserId = 22 };
        SetupQueueWith([draft]);
        SetupClearDrafts();

        _mockScriptApi.Setup(a => a.UpdateAsync(It.IsAny<UpdateScriptRequest>()))
            .ReturnsAsync(new ScriptResponse { Id = 42, Title = "Edit" });

        var result = await _sut.SyncAllAsync();

        Assert.Equal(1, result.SyncedCount);
        Assert.Equal(42, result.LastScriptId);
        _mockScriptApi.Verify(a => a.UpdateAsync(It.Is<UpdateScriptRequest>(r => r.Id == 42)), Times.Once);
    }

    [Fact]
    public async Task SyncAllAsync_StopsOnFailure_KeepsRemainingInQueue()
    {
        var d1 = new OfflineDraft { LocalId = "a", Title = "First", UserId = 22 };
        var d2 = new OfflineDraft { LocalId = "b", Title = "Second", UserId = 22 };
        SetupQueueWith([d1, d2]);

        _mockScriptApi.Setup(a => a.CreateAsync(It.IsAny<CreateScriptRequest>()))
            .ThrowsAsync(new HttpRequestException("offline"));

        string? savedJson = null;
        _mockJs.Setup(j => j.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "bcvOffline.saveDrafts", It.IsAny<object[]>()))
            .Callback<string, object[]>((_, args) => savedJson = args[0] as string)
            .ReturnsAsync(Mock.Of<Microsoft.JSInterop.Infrastructure.IJSVoidResult>());

        var result = await _sut.SyncAllAsync();

        Assert.Equal(0, result.SyncedCount);
        // Queue should NOT have been cleared — saveDrafts not called for partial=0
        _mockJs.Verify(j => j.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
            "bcvOffline.clearDrafts", It.IsAny<object[]>()), Times.Never);
    }

    [Fact]
    public async Task SyncAllAsync_ClearsQueue_WhenAllSynced()
    {
        var draft = new OfflineDraft { LocalId = "a", Title = "Done", UserId = 22 };
        SetupQueueWith([draft]);
        SetupClearDrafts();

        _mockScriptApi.Setup(a => a.CreateAsync(It.IsAny<CreateScriptRequest>()))
            .ReturnsAsync(new ScriptResponse { Id = 10 });

        await _sut.SyncAllAsync();

        _mockJs.Verify(j => j.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
            "bcvOffline.clearDrafts", It.IsAny<object[]>()), Times.Once);
    }

    // --- Helpers ---

    private void SetupEmptyQueue() =>
        _mockJs.Setup(j => j.InvokeAsync<string?>("bcvOffline.loadDrafts", It.IsAny<object[]>()))
            .ReturnsAsync((string?)null);

    private void SetupQueueWith(List<OfflineDraft> drafts) =>
        _mockJs.Setup(j => j.InvokeAsync<string?>("bcvOffline.loadDrafts", It.IsAny<object[]>()))
            .ReturnsAsync(JsonSerializer.Serialize(drafts));

    private void SetupClearDrafts() =>
        _mockJs.Setup(j => j.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "bcvOffline.clearDrafts", It.IsAny<object[]>()))
            .ReturnsAsync(Mock.Of<Microsoft.JSInterop.Infrastructure.IJSVoidResult>());
}
