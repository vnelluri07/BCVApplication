using System.Text;
using System.Xml;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedController : ControllerBase
{
    private readonly IScriptService _scriptService;
    public FeedController(IScriptService scriptService) => _scriptService = scriptService;

    [HttpGet("rss")]
    [Produces("application/rss+xml")]
    public async Task<IActionResult> Rss(CancellationToken cancellationToken)
    {
        var scripts = await _scriptService.GetPublishedScriptsAsync(cancellationToken);

        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, Async = true });

        await writer.WriteStartDocumentAsync();
        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");
        writer.WriteStartElement("channel");

        writer.WriteElementString("title", "Beers Cheers & Vasis");
        writer.WriteElementString("description", "Stories, thoughts, and everything in between");
        writer.WriteElementString("language", "en-us");

        foreach (var s in scripts.Take(20))
        {
            writer.WriteStartElement("item");
            writer.WriteElementString("title", s.Title);
            writer.WriteElementString("pubDate", (s.PublishedDate ?? s.CreatedDate)?.ToString("R") ?? "");
            writer.WriteElementString("guid", s.Id.ToString());
            var excerpt = System.Text.RegularExpressions.Regex.Replace(s.Content ?? "", "<[^>]+>", " ");
            if (excerpt.Length > 300) excerpt = excerpt[..300] + "…";
            writer.WriteElementString("description", excerpt);
            writer.WriteEndElement();
        }

        writer.WriteEndElement(); // channel
        writer.WriteEndElement(); // rss
        await writer.WriteEndDocumentAsync();
        await writer.FlushAsync();

        return Content(sb.ToString(), "application/rss+xml", Encoding.UTF8);
    }
}
