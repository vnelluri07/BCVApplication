using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Users;
public record EditUserViewModel
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string NewPassword { get; set; }
    public bool IsActive { get; set; }

    public ImmutableList<string> ErrorMessages => Messages.ErrorMessages;
    public ImmutableList<string> SuccessMessages => Messages.SuccessMessages;
    public MessageViewModel Messages { get; init; } = new();
    public ImmutableList<RoleViewModel> Roles { get; init; } = ImmutableList<RoleViewModel>.Empty;

    public EditUserViewModel Message(MessageViewModel vm)
    {
        return this with
        {
            Messages = vm
        };
    }


    public EditUserViewModel()
    {
        UserName = string.Empty;
        UserEmail = string.Empty;
        NewPassword = string.Empty;
        IsActive = true;
    }

    public EditUserViewModel(UserViewModel vm)
    {
        UserName = vm.UserName;
        UserEmail = vm.UserEmail;
        NewPassword = string.Empty;
        IsActive = vm.IsActive;
        Roles = vm.Roles;
    }
}
