using System.Collections.Immutable;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;
using BeersCheersVasis.UI.ViewModels.UserManagement.Users;

namespace BlazorApp3.Pages.User;

public class UserController
{
    private readonly IApiClient _api;

    public UserController(IApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<UserPageViewModel> OnInitializeAsync()
    {
        var model = new UserPageViewModel();
        model = await RefreshUsers(model);
        return model;
    }

    public async Task<UserPageViewModel> RefreshUsers(UserPageViewModel model)
    {
        try
        {
            var users = await _api.UserApi.ListAsync();

            return model.SetUsers(users.Select(u =>
                new UserViewModel(
                    u.Id,
                    u.Name,
                    u.Email,
                    u.IsActive ?? 0)).ToList());
        }

        catch (Exception ex)
        {
            model = model.Message(model.Messages.SetErrorMessages(ex.Message));
            return model;
        }

    }
}

