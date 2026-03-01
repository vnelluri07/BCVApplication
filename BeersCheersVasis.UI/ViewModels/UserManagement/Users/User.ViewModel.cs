using System.Collections.Immutable;
using BeersCheersVasis.UI.ViewModels.UserManagement.Roles;

namespace BeersCheersVasis.UI.ViewModels.UserManagement.Users
{
    public record UserViewModel
    {
        public int Id { get; init; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int IsActive { get; set; }
        public bool IsSelected { get; init; }
        public ImmutableList<RoleViewModel> Roles { get; init; }
        public UserViewModel()
        {
            Id = 0;
            UserName = String.Empty;
            UserEmail = String.Empty;
            IsActive = 0;
            IsSelected = false;

            Roles = ImmutableList<RoleViewModel>.Empty;
        }

        public UserViewModel(int? id, string? userName, string? userEmail, int isActive, ImmutableList<RoleViewModel> roles)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(userName, nameof(userName));
            ArgumentNullException.ThrowIfNull(userEmail, nameof(userEmail));
            Id = (int)id;
            UserName = userName;
            UserEmail = userEmail;
            IsActive = isActive;

            Roles = roles;
        }

        public UserViewModel(int? id, string? userName, string? userEmail, int isActive)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(userName, nameof(userName));
            ArgumentNullException.ThrowIfNull(userEmail, nameof(userEmail));
            Id = (int)id;
            UserName = userName;
            UserEmail = userEmail;
            IsActive = isActive;
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
