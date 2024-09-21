using IsActiveRouteTagHelper.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IsActiveRouteTagHelper;

/// <summary>
/// アンカータグに is-active-route 属性を付与することで、該当のページを開いているときに
/// bootstrap の active クラスを追加します。
/// </summary>
/// <param name="generator"></param>
[HtmlTargetElement(Attributes = "is-active-route")]
public class IsActiveRouteTagHelper(IHtmlGenerator generator) : AnchorTagHelper(generator)
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var routeValues = ViewContext.RouteData.Values;
        var currentArea = routeValues.GetStringValueOrDefault("area");
        var currentPage = routeValues.GetStringValueOrDefault("page");
        var currentController = routeValues.GetStringValueOrDefault("controller");
        var currentAction = routeValues.GetStringValueOrDefault("action");

        bool isCurrentRoute;
        if (!string.IsNullOrEmpty(currentPage))
        {
            // Razor Pages
            isCurrentRoute = string.IsNullOrEmpty(currentArea) switch
            {
                true => IsEqualsIgnoreCase(Page, currentPage),
                false => IsEqualsIgnoreCase(Area, currentArea) && IsEqualsIgnoreCase(Page, currentPage),
            };
        }
        else
        {
            // MVC Views
            isCurrentRoute = (
                    string.IsNullOrEmpty(currentArea),
                    string.IsNullOrEmpty(currentController),
                    string.IsNullOrEmpty(currentAction)
                ) switch
                {
                    (true, true, true) => false,
                    (true, true, false) => IsEqualsIgnoreCase(Action, currentAction),
                    (true, false, true) => IsEqualsIgnoreCase(Controller, currentController),
                    (true, false, false) => IsEqualsIgnoreCase(Controller, currentController) &&
                                            IsEqualsIgnoreCase(Action, currentAction),
                    (false, true, true) => IsEqualsIgnoreCase(Area, currentArea),
                    (false, true, false) => IsEqualsIgnoreCase(Area, currentArea) &&
                                            IsEqualsIgnoreCase(Action, currentAction),
                    (false, false, true) => IsEqualsIgnoreCase(Area, currentArea) &&
                                            IsEqualsIgnoreCase(Controller, currentController),
                    (false, false, false) => IsEqualsIgnoreCase(Area, currentArea) &&
                                             IsEqualsIgnoreCase(Controller, currentController) &&
                                             IsEqualsIgnoreCase(Action, currentAction)
                };
        }


        // remove existing active class name
        // and then add active if isCurrentRoute
        var classNames =
            output.Attributes.GetClassAttributes()
                .Except(["active"], StringComparer.OrdinalIgnoreCase)
                .Concat(isCurrentRoute ? ["active"] : []);

        // set class attribute
        output.Attributes.SetAttribute("class", $"{string.Join(' ', classNames)}");
    }

    private static bool IsEqualsIgnoreCase(string s1, string s2)
    {
        return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
    }
}