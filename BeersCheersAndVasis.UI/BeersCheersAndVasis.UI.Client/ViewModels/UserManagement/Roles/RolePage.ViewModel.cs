using BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Roles
{
    public record RolesPageViewModel
    {
        public RoleViewModel? SelectedRole => Roles.FirstOrDefault(p => p.IsSelected);

        public ImmutableList<string> ErrorMessages => Messages.ErrorMessages;
        public ImmutableList<string> SuccessMessages => Messages.SuccessMessages;

        public MessageViewModel Messages { get; init; } = new MessageViewModel();

        public ImmutableList<RoleViewModel> Roles { get; init; } = ImmutableList<RoleViewModel>.Empty;
        public ImmutableList<PermissionViewModel> Permissions { get; init; } = ImmutableList<PermissionViewModel>.Empty;

        public RolesPageViewModel SetRoles(List<RoleViewModel> roles)
        {
            return this with
            {
                Roles = roles.ToImmutableList()
            };
        }

        public RolesPageViewModel SetPermissions(List<PermissionViewModel> perms)
        {
            return this with
            {
                Permissions = perms.ToImmutableList()
            };
        }

        public RolesPageViewModel SelectRole(RoleViewModel role)
        {
            var model = this.EditRoles(r => true, r => r with
            {
                IsSelected = false
            });

            model = model.EditRole(role.Id, r =>
                r with { IsSelected = true }
            );

            return model;
        }

        public RolesPageViewModel EditRoles(Func<RoleViewModel, bool> predicate, Func<RoleViewModel, RoleViewModel> transform)
        {
            return this with
            {
                Roles = Roles.Select(p => predicate(p) ? transform(p) : p).ToImmutableList()
            };
        }

        public RolesPageViewModel EditRole(Guid id, Func<RoleViewModel, RoleViewModel> transform)
        {
            return EditRoles(r => r.Id == id, transform);
        }

        public RolesPageViewModel EditRole(RoleViewModel role)
        {
            return EditRole(role.Id, _ => role);
        }

        public RolesPageViewModel Message(MessageViewModel vm)
        {
            return this with
            {
                Messages = vm
            };
        }
    }
}
