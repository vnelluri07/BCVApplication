using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;

public record EditPermissionViewModel
{
    public string PermissionName { get; set; }
    public string PermissionDescription { get; set; }
    public string PermissionLink { get; set; }
    public bool? IsActive { get; set; }
    public string? ParentPermissionName { get; set; }

    // can this be extracted into an ErrorViewModel?
    //TODO: Add later
    //public MessageViewModel Messages = new MessageViewModel();
    public ImmutableList<string> ErrorMessages { get; init; } = ImmutableList<string>.Empty;
    public ImmutableList<string> SuccessMessages { get; init; } = ImmutableList<string>.Empty;
    public ImmutableList<RoleViewModel> Roles { get; init; } = ImmutableList<RoleViewModel>.Empty;
    // ---


    public EditPermissionViewModel()
    {
        PermissionName = string.Empty;
        PermissionDescription = string.Empty;
        PermissionLink = string.Empty;
        IsActive = true;
        ParentPermissionName = null;  //todo: probably fix
    }

    public EditPermissionViewModel(PermissionViewModel vm)
    {
        PermissionName = vm.PermissionName;
        PermissionDescription = vm.PermissionDescription;
        PermissionLink = vm.PermissionLink;
        IsActive = vm.IsActive;
        ParentPermissionName = vm.ParentPermissionName;
    }

    public EditPermissionViewModel(string name, string description, string link, string? parentPermission)
    {
        PermissionName = name;
        PermissionDescription = description;
        PermissionLink = link;
        ParentPermissionName = parentPermission;
    }

    public EditPermissionViewModel ClearErrors()
    {
        return this with
        {
            ErrorMessages = ImmutableList<string>.Empty
        };
    }

    public EditPermissionViewModel SetErrorMessages(string success)
    {
        return this with
        {
            ErrorMessages = ErrorMessages.AddRange(new List<string>() { success })
        };
    }

    public EditPermissionViewModel SetErrorMessages(List<string> successes)
    {
        return this with
        {
            ErrorMessages = ErrorMessages.AddRange(successes)
        };
    }

    public EditPermissionViewModel SetSuccessMessages(string success)
    {
        return this with
        {
            SuccessMessages = SuccessMessages.AddRange(new List<string>() { success })
        };
    }

    public EditPermissionViewModel SetSuccessMessages(List<string> successes)
    {
        return this with
        {
            SuccessMessages = SuccessMessages.AddRange(successes)
        };
    }

    public EditPermissionViewModel ClearSuccessMessages()
    {
        return this with
        {
            SuccessMessages = ImmutableList<string>.Empty
        };
    }
}
