using BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Roles
{
    public record EditRoleViewModel
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public ImmutableList<PermissionViewModel> RolePermissions { get; init; }

        // can this be extracted into an ErrorViewModel?
        public ImmutableList<string> ErrorMessages { get; init; } = ImmutableList<string>.Empty;
        public ImmutableList<string> SuccessMessages { get; init; } = ImmutableList<string>.Empty;
        // ---

        public EditRoleViewModel()
        {
            RoleName = string.Empty;
            RoleDescription = string.Empty;
            IsActive = true;
            RolePermissions = ImmutableList<PermissionViewModel>.Empty;
        }

        public EditRoleViewModel(RoleViewModel vm)
        {
            RoleName = vm.RoleName;
            RoleDescription = vm.RoleDescription;
            IsActive = vm.IsActive;
            //TODO: add later
            //RolePermissions = vm.RolePermissions;
        }

        public EditRoleViewModel(string name, string description, ImmutableList<PermissionViewModel> permissions)
        {
            RoleName = name;
            RoleDescription = description;
            RolePermissions = permissions;
        }

        public EditRoleViewModel ClearErrors()
        {
            return this with
            {
                ErrorMessages = ImmutableList<string>.Empty
            };
        }
        public EditRoleViewModel SetErrorMessages(string success)
        {
            return this with
            {
                ErrorMessages = ErrorMessages.AddRange(new List<string>() { success })
            };
        }

        public EditRoleViewModel SetErrorMessages(List<string> successes)
        {
            return this with
            {
                ErrorMessages = ErrorMessages.AddRange(successes)
            };
        }


        public EditRoleViewModel SetSuccessMessages(string success)
        {
            return this with
            {
                SuccessMessages = SuccessMessages.AddRange(new List<string>() { success })
            };
        }

        public EditRoleViewModel SetSuccessMessages(List<string> successes)
        {
            return this with
            {
                SuccessMessages = SuccessMessages.AddRange(successes)
            };
        }

        public EditRoleViewModel ClearSuccessMessages()
        {
            return this with
            {
                SuccessMessages = ImmutableList<string>.Empty
            };
        }
    }
}
