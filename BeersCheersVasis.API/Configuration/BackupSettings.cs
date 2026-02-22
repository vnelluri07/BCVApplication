namespace BeersCheersVasis.API.Configuration;

public sealed class BackupSettings
{
    public GitHubBackupSettings GitHub { get; set; } = new();
    public GoogleDriveBackupSettings GoogleDrive { get; set; } = new();
}

public sealed class GitHubBackupSettings
{
    public string PersonalAccessToken { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Repo { get; set; } = "bcv-content-backup";
}

public sealed class GoogleDriveBackupSettings
{
    public string ServiceAccountJson { get; set; } = string.Empty;
    public string FolderId { get; set; } = string.Empty;
}
