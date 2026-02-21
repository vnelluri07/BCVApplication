using BeersCheersAndVasis.UI.Components.Pages.Script;
using BeersCheersAndVasis.UI.ViewModels.Script;
using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Pages.Script;

public partial class Script1
{
    private ScriptPageViewModel _model { get; set; } = new();
    [Inject] private ScriptController Controller { get; set; } = default!;

    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = new();

    protected async Task OnInitializedAsync()
    {
        await InitPage();
    }

    private async Task InitPage()
    {
        if (Controller == null)
        {
            throw new NullReferenceException(nameof(ScriptController));
        }

        //isLoading = true;
        _model = await Controller.OnInitializeAsync();
        //isLoading = false;

        //DisplayMessages();

        //await InvokeAsync(StateHasChanged);
    }
}