using BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;
using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement
{
    public record PermissionPageViewModel
    {
        public PermissionViewModel? SelectedPermission => Permissions.FirstOrDefault(p => p.IsSelected);

        // can this be extracted into an ErrorViewModel?
        public ImmutableList<string> ErrorMessages { get; init; } = ImmutableList<string>.Empty;
        public ImmutableList<string> SuccessMessages { get; init; } = ImmutableList<string>.Empty;
        // ---

        public ImmutableList<PermissionViewModel> Permissions { get; init; } = ImmutableList<PermissionViewModel>.Empty;
        public ImmutableList<RoleViewModel> Roles { get; init; } = ImmutableList<RoleViewModel>.Empty;

        public PermissionPageViewModel SetPermissions(List<PermissionViewModel> permissions)
        {
            return this with
            {
                Permissions = permissions.ToImmutableList()
            };
        }

        public PermissionPageViewModel SetRoles(List<RoleViewModel> roles)
        {
            return this with
            {
                Roles = roles.ToImmutableList()
            };
        }

        public PermissionPageViewModel EditPermissions(Func<PermissionViewModel, bool> predicate, Func<PermissionViewModel, PermissionViewModel> transform)
        {
            return this with
            {
                Permissions = Permissions.Select(p => predicate(p) ? transform(p) : p).ToImmutableList()
            };
        }

        public PermissionPageViewModel EditPermission(Guid id, Func<PermissionViewModel, PermissionViewModel> transform)
        {
            return EditPermissions(p => p.PermissionId == id, transform);
        }

        public PermissionPageViewModel EditPermission(PermissionViewModel perm)
        {
            return EditPermission(perm.PermissionId, _ => perm);
        }

        public PermissionPageViewModel SelectPermission(PermissionViewModel perm)
        {
            var model = this.EditPermissions(p => true, p => p with
            {
                IsSelected = false
            });

            model = model.EditPermission(perm.PermissionId, p =>
                p with { IsSelected = true }
            );

            return model;
        }

        // can we extract this into an ErrorsViewModel?
        public PermissionPageViewModel ClearErrors()
        {
            return this with
            {
                ErrorMessages = ImmutableList<string>.Empty
            };
        }

        public PermissionPageViewModel SetErrorMessages(string success)
        {
            return this with
            {
                ErrorMessages = ErrorMessages.AddRange(new List<string>() { success })
            };
        }

        public PermissionPageViewModel SetErrorMessages(List<string> successes)
        {
            return this with
            {
                ErrorMessages = ErrorMessages.AddRange(successes)
            };
        }

        public PermissionPageViewModel SetSuccessMessages(string success)
        {
            return this with
            {
                SuccessMessages = SuccessMessages.AddRange(new List<string>() { success })
            };
        }

        public PermissionPageViewModel SetSuccessMessages(List<string> successes)
        {
            return this with
            {
                SuccessMessages = SuccessMessages.AddRange(successes)
            };
        }

        public PermissionPageViewModel ClearSuccessMessages()
        {
            return this with
            {
                SuccessMessages = ImmutableList<string>.Empty
            };
        }
    }
}
