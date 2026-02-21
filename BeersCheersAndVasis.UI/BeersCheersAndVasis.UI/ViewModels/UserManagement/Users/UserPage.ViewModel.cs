using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Users;
public record UserPageViewModel
{
    public UserViewModel? SelectedUser => Users.FirstOrDefault(p => p.IsSelected);

    public ImmutableList<string> ErrorMessages => Messages.ErrorMessages;
    public ImmutableList<string> SuccessMessages => Messages.SuccessMessages;
    public MessageViewModel Messages { get; init; } = new MessageViewModel();

    public ImmutableList<UserViewModel> Users { get; init; } = ImmutableList<UserViewModel>.Empty;
    public ImmutableList<RoleViewModel> Roles { get; init; } = ImmutableList<RoleViewModel>.Empty;

    public UserPageViewModel Message(MessageViewModel vm)
    {
        return this with
        {
            Messages = vm
        };
    }

    public UserPageViewModel SetRoles(List<RoleViewModel> roles)
    {
        return this with
        {
            Roles = roles.ToImmutableList()
        };
    }

    public UserPageViewModel SetUsers(List<UserViewModel> users)
    {
        return this with
        {
            Users = users.ToImmutableList()
        };
    }

    public UserPageViewModel EditUsers(Func<UserViewModel, bool> predicate, Func<UserViewModel, UserViewModel> transform)
    {
        return this with
        {
            Users = Users.Select(p => predicate(p) ? transform(p) : p).ToImmutableList()
        };
    }

    public UserPageViewModel EditUser(Guid id, Func<UserViewModel, UserViewModel> transform)
    {
        return EditUsers(u => u.Id == id, transform);
    }

    public UserPageViewModel EditUser(UserViewModel user)
    {
        return EditUser(user.Id, _ => user);
    }

    public UserPageViewModel SelectUser(UserViewModel user)
    {
        var model = EditUsers(u => true, u => u with
        {
            IsSelected = false
        });

        model = model.EditUser(user.Id, u =>
            u with { IsSelected = true }
        );

        return model;
    }
}