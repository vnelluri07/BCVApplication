using BeersCheersVasis.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public partial class ShareController : ControllerBase
{
    private readonly IScriptRepository _scriptRepository;
    private const string BlazorBaseUrl = "http://beerscheersvasis.runasp.net";
    private const string SiteName = "Beers Cheers & Vasis";

    public ShareController(IScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository;
    }

    [HttpGet("script/{id:int}")]
    public async Task<IActionResult> Script(int id, CancellationToken cancellationToken)
    {
        try
        {
            var script = await _scriptRepository.GetScriptAsync(id, cancellationToken);
            var title = script.Title ?? SiteName;
            var description = GetPlainText(script.Content, 160);
            var blazorUrl = $"{BlazorBaseUrl}/read/script/{id}";

            return Content(BuildOgHtml(title, description, blazorUrl), "text/html");
        }
        catch
        {
            return Redirect(BlazorBaseUrl);
        }
    }

    private static string BuildOgHtml(string title, string description, string redirectUrl)
    {
        var safeTitle = System.Net.WebUtility.HtmlEncode(title);
        var safeDesc = System.Net.WebUtility.HtmlEncode(description);

        return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8" />
                <title>{safeTitle} — {SiteName}</title>
                <meta property="og:title" content="{safeTitle}" />
                <meta property="og:description" content="{safeDesc}" />
                <meta property="og:type" content="article" />
                <meta property="og:url" content="{redirectUrl}" />
                <meta property="og:site_name" content="{SiteName}" />
                <meta name="twitter:card" content="summary" />
                <meta name="twitter:title" content="{safeTitle}" />
                <meta name="twitter:description" content="{safeDesc}" />
                <meta http-equiv="refresh" content="0;url={redirectUrl}" />
            </head>
            <body>
                <p>Redirecting to <a href="{redirectUrl}">{safeTitle}</a>...</p>
            </body>
            </html>
            """;
    }

    private static string GetPlainText(string? html, int maxLength)
    {
        if (string.IsNullOrEmpty(html)) return "This is the stage for Vasi Nelluri's madness";
        var text = StripHtmlRegex().Replace(html, " ").Trim();
        text = System.Net.WebUtility.HtmlDecode(text);
        text = WhitespaceRegex().Replace(text, " ");
        return text.Length > maxLength ? text[..maxLength] + "…" : text;
    }

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex StripHtmlRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
