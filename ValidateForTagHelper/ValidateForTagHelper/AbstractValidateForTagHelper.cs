using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ValidateForTagHelper;

/// <summary>
/// common base class for validate-for TagHelper classes
/// </summary>
public abstract class AbstractValidateForTagHelper : TagHelper
{
    protected const string ValidateForAttrName = "validate-for";
    
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;
    
    /// <summary>
    /// validate-for に指定されたモデルのプロパティ名です
    /// </summary>
    protected string? ValidatePropertyName { get; set; }
    
    /// <summary>
    /// validate-for に指定されたプロパティの ModelState です
    /// </summary>
    protected ModelStateEntry? ValidatePropertyEntry { get; set; }

    public override void Init(TagHelperContext context)
    {
        if (context.AllAttributes.TryGetAttribute(ValidateForAttrName, out var propertyNameAttr))
        {
            ValidatePropertyName = propertyNameAttr.Value.ToString();
        }

        if (!string.IsNullOrEmpty(ValidatePropertyName))
        {
            if (ViewContext.ModelState.TryGetValue(ValidatePropertyName, out var modelStateEntry))
            {
                ValidatePropertyEntry = modelStateEntry;
            }
        }
    }

    protected string[] GetClassAttributes(TagHelperContext context)
    {
        return context.AllAttributes.TryGetAttribute("class", out var classAttr) switch
        {
            true => classAttr.Value.ToString()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [],
            false => [],
        };
    }
}