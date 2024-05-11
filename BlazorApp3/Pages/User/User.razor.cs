using BeersCheersVasis.UI.ViewModels.UserManagement.Users;
using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Pages.User;

public partial class User
{
    private UserPageViewModel _model { get; set; } = new UserPageViewModel();
    [Inject] private UserController Controller { get; set; } = default!;
    //[Inject] private ISnackbar Snackbar { get; set; } = default!;

    private bool isLoading = false;
    private bool _isViewingActive = true;

    protected async override Task OnInitializedAsync()
    {
        await InitPage();
    }

    private async Task InitPage()
    {
        if (Controller == null)
        {
            throw new NullReferenceException(nameof(UserController));
        }

        isLoading = true;
        _model = await Controller.OnInitializeAsync();
        isLoading = false;

        //DisplayMessages();

        await InvokeAsync(StateHasChanged);
    }

    //private void DisplayMessages()
    //{
    //    if (_model.ErrorMessages.Any())
    //    {
    //        foreach (var error in _model.ErrorMessages)
    //        {
    //            Snackbar.Add(error, Severity.Error);
    //        }

    //        _model = _model.Message(_model.Messages.ClearErrors());
    //    }

    //    if (_model.SuccessMessages.Any())
    //    {
    //        foreach (var success in _model.SuccessMessages)
    //        {
    //            Snackbar.Add(success, Severity.Success);
    //        }

    //        _model = _model.Message(_model.Messages.ClearSuccessMessages());
    //    }
    //}
}
