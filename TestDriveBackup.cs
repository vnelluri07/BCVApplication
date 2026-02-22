using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

var json = args[0];
Console.WriteLine($"JSON length: {json.Length}");

try
{
    var credential = GoogleCredential.FromJson(json).CreateScoped(DriveService.Scope.DriveFile);
    Console.WriteLine("Credential created OK");

    using var service = new DriveService(new BaseClientService.Initializer
    {
        HttpClientInitializer = credential,
        ApplicationName = "BCVApp"
    });

    var about = await service.About.Get().ExecuteAsync();
    Console.WriteLine($"Drive connected. User: {about.User?.DisplayName ?? "service account"}");

    // Try creating a test file
    var fileMetadata = new Google.Apis.Drive.v3.Data.File
    {
        Name = "BCV-Test-Backup",
        MimeType = "application/vnd.google-apps.document"
    };
    using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("<html><body><h1>Test</h1></body></html>"));
    var request = service.Files.Create(fileMetadata, stream, "text/html");
    request.Fields = "id, webViewLink";
    var result = await request.UploadAsync();
    
    if (result.Status == Google.Apis.Upload.UploadStatus.Completed)
    {
        Console.WriteLine($"SUCCESS! File ID: {request.ResponseBody.Id}");
        Console.WriteLine($"URL: {request.ResponseBody.WebViewLink}");
        // Clean up
        await service.Files.Delete(request.ResponseBody.Id).ExecuteAsync();
        Console.WriteLine("Test file deleted.");
    }
    else
    {
        Console.WriteLine($"UPLOAD FAILED: {result.Status}");
        Console.WriteLine($"Error: {result.Exception?.Message}");
        Console.WriteLine($"Full: {result.Exception}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    Console.WriteLine(ex.ToString());
}
