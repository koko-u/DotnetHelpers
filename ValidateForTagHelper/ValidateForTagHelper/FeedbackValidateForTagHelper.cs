using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ValidateForTagHelper;

/// <summary>
/// 入力の validate-for をセットで &lt;div validate-for="Name"&gt;&lt;/div&gt; などと記述すると
/// &lt;div id="NameFeedback" class="invalid-feedback"&gt;
///   Name is required
/// &lt;/div&gt;
/// などと、エラーメッセージを表示するようにしてくれる
/// </summary>
[HtmlTargetElement("div", Attributes = ValidateForAttrName)]
[HtmlTargetElement("span", Attributes = ValidateForAttrName)]
public class FeedbackValidateForTagHelper : AbstractValidateForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.SetAttribute("id", $"{ValidatePropertyName}Feedback");

        var classAttrs = string.Join(' ', GetClassAttributes(context).Concat(["invalid-feedback"]));
        output.Attributes.SetAttribute("class", classAttrs);

        var errorMessage = ValidatePropertyEntry?.Errors.Select(entry => entry.ErrorMessage).FirstOrDefault();
        output.Content.SetContent(errorMessage);
    }
}