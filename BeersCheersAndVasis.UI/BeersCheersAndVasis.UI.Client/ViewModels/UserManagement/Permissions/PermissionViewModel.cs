namespace BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;
public record PermissionViewModel
{
    public Guid PermissionId { get; }
    public string PermissionName { get; set; }
    public string PermissionDescription { get; set; }
    public bool IsActive { get; set; }
    public string PermissionLink { get; set; }
    public string? ParentPermissionName { get; set; }


    public bool IsSelected { get; init; }

    public PermissionViewModel(Guid? id, string? permissionName, string? permissionDescription, bool isActive, string? permissionLink, string? parentPermissionName)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(permissionName, nameof(permissionName));
        ArgumentNullException.ThrowIfNull(permissionDescription, nameof(permissionDescription));
        ArgumentNullException.ThrowIfNull(permissionLink, nameof(permissionLink));
        PermissionId = (Guid)id;
        PermissionName = permissionName;
        PermissionDescription = permissionDescription;
        IsActive = isActive;
        PermissionLink = permissionLink;
        ParentPermissionName = parentPermissionName;
        IsSelected = false;
    }

    public List<string> SearchFields
    {
        get
        {
            var fields = new List<string>()
            {
                PermissionName!,
                PermissionDescription!,
                PermissionLink!
            };
            return fields;
        }
    }
}
