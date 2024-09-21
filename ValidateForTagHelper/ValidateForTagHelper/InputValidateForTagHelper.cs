using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ValidateForTagHelper;

/// <summary>
/// input や textarea に validate-for="Name" などと付与することで、
/// 対象のプロパティ(Name)にエラーがある場合に is-invalid クラスを付与する
///
/// aria-describedby 属性を [PropertyName]Feedback の命名で付与する
/// </summary>
[HtmlTargetElement("input", Attributes = ValidateForAttrName)]
[HtmlTargetElement("textarea", Attributes = ValidateForAttrName)]
[HtmlTargetElement("select", Attributes = ValidateForAttrName)]
public class InputValidateForTagHelper : AbstractValidateForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrEmpty(ValidatePropertyName))
        {
            return;
        }
    
        // add aria-describedBy 
        output.Attributes.SetAttribute("aria-describedby", $"{ValidatePropertyName}Feedback");
    
        // バリデーションが実行されていない場合は aria-describedby を追加して終わり
        if (ValidatePropertyEntry is null)
        {
            return;
        }
        
        // バリデーションの結果に応じて is-valid / is-invalid を判定して、すでにある class 属性に追加する
        var validationClass = ValidatePropertyEntry.ValidationState switch
        {
            ModelValidationState.Valid => "is-valid",
            ModelValidationState.Skipped => "is-valid",
            ModelValidationState.Invalid => "is-invalid",
            ModelValidationState.Unvalidated => "",
            _ => throw new NotSupportedException($"Unexpected model validation state {ValidatePropertyEntry.ValidationState}"),
        };
        
        var classNames = 
            GetClassAttributes(context)
                .Except(["is-valid", "is-invalid"], StringComparer.OrdinalIgnoreCase)
                .Concat([validationClass])
                .ToList();
    
        output.Attributes.SetAttribute("class", string.Join(' ', classNames));
    }
}