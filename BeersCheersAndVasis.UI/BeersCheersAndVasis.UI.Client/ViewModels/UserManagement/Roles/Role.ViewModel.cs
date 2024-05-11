using BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;
using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Roles
{
    public record RoleViewModel
    {
        public Guid Id { get; }
        public bool IsSelected { get; init; }
        public string RoleName { get; init; }
        public string RoleDescription { get; init; }
        public bool IsActive { get; set; }
        public ImmutableList<PermissionViewModel> RolePermissions { get; init; }

        public RoleViewModel(Guid? id, string? roleName, string? roleDescription, bool isActive, ImmutableList<PermissionViewModel> permissions)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
            ArgumentNullException.ThrowIfNull(roleDescription, nameof(roleDescription));
            Id = (Guid)id;
            RoleName = roleName;
            RoleDescription = roleDescription;
            IsActive = isActive;
            RolePermissions = permissions;

            IsSelected = false;
        }

        public List<string> SearchFields
        {
            get
            {
                var fields = new List<string>()
                {
                    RoleName!,
                    RoleDescription!,
                };
                return fields;
            }
        }
    }
}
