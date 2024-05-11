using System.Collections.Immutable;
using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Users
{
    public record UserViewModel
    {
        public Guid Id { get; init; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; init; }
        public ImmutableList<RoleViewModel> Roles { get; init; }
        public UserViewModel()
        {
            Id = Guid.NewGuid();
            UserName = String.Empty;
            UserEmail = String.Empty;
            IsActive = true;
            IsSelected = false;

            Roles = ImmutableList<RoleViewModel>.Empty;
        }

        public UserViewModel(Guid? id, string? userName, string? userEmail, bool isActive, ImmutableList<RoleViewModel> roles)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(userName, nameof(userName));
            ArgumentNullException.ThrowIfNull(userEmail, nameof(userEmail));
            Id = (Guid)id;
            UserName = userName;
            UserEmail = userEmail;
            IsActive = isActive;

            Roles = roles;
        }

        public List<string> SearchFields
        {
            get
            {
                var fields = new List<string>()
                {
                    UserName!,
                    UserEmail!,
                };
                return fields;
            }
        }
    }
}
