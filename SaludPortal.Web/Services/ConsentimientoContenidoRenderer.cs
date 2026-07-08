using Ganss.Xss;
using Markdig;
using Microsoft.AspNetCore.Components;
using SaludPortal.Application.Models.Consentimiento;

namespace SaludPortal.Web.Services;

public class ConsentimientoContenidoRenderer
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    private static readonly HtmlSanitizer Sanitizer = CreateSanitizer();

    public MarkupString Render(string? texto, FormatoContenidoConsentimiento formato = FormatoContenidoConsentimiento.Html)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return new MarkupString(string.Empty);
        }

        if (formato == FormatoContenidoConsentimiento.TextoPlano)
        {
            return new MarkupString(string.Empty);
        }

        var html = formato == FormatoContenidoConsentimiento.Markdown
            ? Markdown.ToHtml(texto, MarkdownPipeline)
            : texto;

        return new MarkupString(Sanitizer.Sanitize(html));
    }

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.UnionWith(
        [
            "p", "br", "strong", "b", "em", "i", "u",
            "ul", "ol", "li",
            "h1", "h2", "h3", "h4",
            "a", "hr", "blockquote"
        ]);

        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedAttributes.UnionWith(["href", "title", "target", "rel"]);

        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.UnionWith(["http", "https", "mailto"]);

        sanitizer.AllowedAtRules.Clear();

        return sanitizer;
    }
}
