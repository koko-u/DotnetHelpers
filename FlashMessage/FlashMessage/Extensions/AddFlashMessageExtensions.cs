using FlashMessage.Contracts;
using FlashMessage.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FlashMessage.Extensions;

public static class AddFlashMessageExtensions
{
    /// <summary>
    /// Register flash message service and adds services required for flash message to work.
    /// </summary>
    /// <param name="services"></param>
    public static void AddFlashMessage(this IServiceCollection services)
    {

        // Only register ITempDataProvider or IHttpContextAccessor implementations in none has
        // been registered yet.
        services.TryAddSingleton<ITempDataProvider, CookieTempDataProvider>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Register flash message specific services.
        services.AddScoped<IFlashMessageSerializer, JsonFlashMessageSerializer>();
        services.AddScoped<IFlashMessage, Core.FlashMessage>();
    }

}
