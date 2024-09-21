using System.Net;
using System.Text;
using FlashMessage.Contracts;
using FlashMessage.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FlashMessage.TagHelpers;

/// <summary>
/// Renders flash messages as Twitter Bootstrap Alerts
/// </summary>
/// <param name="flashMessage"></param>
[OutputElementHint("div")]
[HtmlTargetElement("flash-message", TagStructure = TagStructure.WithoutEndTag)]
public class FlashMessageTagHelper(IFlashMessage flashMessage) : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    /// <summary>
    /// Gets / sets if the message should be dismissible.
    /// </summary>
    public bool Dismissable { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;

        var messages = flashMessage.Retrieve();
        foreach (var message in messages)
        {
            var html = RenderFlashMessage(context, message);
            output.Content.AppendHtml(html);
        }
    }

    private IHtmlContent GetIcon(FlashMessageType flashMessageType)
    {
        return flashMessageType switch
        {
            FlashMessageType.Confirmation => new TagBuilder("i")
            {
                Attributes =
                {
                    ["class"] = "bi bi-check-circle"
                }
            },
            FlashMessageType.Danger => new TagBuilder("i")
            {
                Attributes =
                {
                    ["class"] = "bi bi-ban"
                }
            },
            FlashMessageType.Info => new TagBuilder("i")
            {
                Attributes =
                {
                    ["class"] = "bi bi-info-circle"
                }
            },
            FlashMessageType.Warning => new TagBuilder("i")
            {
                Attributes =
                {
                    ["class"] = "bi bi-exclamation-triangle"
                }
            },
            _ => new HtmlString("")
        };
    }

    private IHtmlContent RenderFlashMessage(TagHelperContext context, FlashMessageViewModel message)
    {
        var tagCss = context.AllAttributes["class"].Value.ToString();
        
        var alertCss = message.Type switch
        {
            FlashMessageType.Danger => "alert alert-danger",
            FlashMessageType.Info => "alert alert-info",
            FlashMessageType.Warning => "alert alert-warning",
            FlashMessageType.Confirmation => "alert alert-success",
            _ => throw new NotSupportedException($"Unknown message typ {message.Type}")
        };

        alertCss += $" {tagCss} d-flex align-items-center";

        if (Dismissable)
        {
            alertCss += " alert-dismissible";
        }

        var alertDiv = new TagBuilder("div")
        {
            Attributes =
            {
                ["class"] = alertCss,
                ["role"] = "alert"
            }
        };
        if (Dismissable)
        {
            alertDiv.InnerHtml.AppendHtml(new TagBuilder("button")
            {
                Attributes =
                {
                    ["type"] = "button",
                    ["class"] = "btn-close",
                    ["data-bs-dismiss"] = "alert",
                    ["aria-label"] = "Close"
                }
            });
        }

        alertDiv.InnerHtml.AppendHtml(GetIcon(message.Type));

        if (!string.IsNullOrWhiteSpace(message.Title))
        {
            var title = new TagBuilder("strong");
            title.InnerHtml.Append(message.Title);
            alertDiv.InnerHtml.AppendHtml(title);
        }

        var spanTag = new TagBuilder("span")
        {
            Attributes = { ["class"] = "ms-1" }
        };
        if (message.IsHtml)
        {
            spanTag.InnerHtml.AppendHtml(message.Message);
        }
        else
        {
            spanTag.InnerHtml.Append(message.Message);
        }
        alertDiv.InnerHtml.AppendHtml(spanTag);

        return alertDiv;
    }
}
