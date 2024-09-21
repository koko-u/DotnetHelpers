using FlashMessage.Contracts;
using FlashMessage.ViewModels;

namespace FlashMessage.Extensions;

public static class FlashMessageExtensions
{
    /// <summary>
    /// Queues a flash message for display of the specified type and with the specified message and title.
    /// </summary>
    /// <param name="flashMessage"></param>
    /// <param name="messageType"></param>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="isHtml"></param>
    public static void Queue(this IFlashMessage flashMessage, 
        FlashMessageType messageType, 
        string message,
        string? title = null, 
        bool isHtml = false)
    {
        // Append the new message.
        var messageModel = new FlashMessageViewModel
        {
            IsHtml = isHtml, 
            Message = message,
            Title = title, 
            Type = messageType
        };
        flashMessage.Queue(messageModel);
    }
    
    
    /// <summary>
    /// Formats and queues the passed message as an informational message with title.
    /// </summary>
    /// <param name="flashMessage"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public static void Info(this IFlashMessage flashMessage, string message, string? title = null)
    {
        flashMessage.Queue(FlashMessageType.Info, message, title);
    }
    
    
    /// <summary>
    /// Queues the passed message as a warning message with optional title.
    /// </summary>
    /// <param name="flashMessage"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public static void Warning(this IFlashMessage flashMessage, string message, string? title = null)
    {
        flashMessage.Queue(FlashMessageType.Warning, message, title);
    }
    
    /// <summary>
    /// Queues the passed message as a danger message with title.
    /// </summary>
    /// <param name="flashMessage"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public static void Danger(this IFlashMessage flashMessage, string message, string? title = null)
    {
        flashMessage.Queue(FlashMessageType.Danger, message, title);
    }

    /// <summary>
    /// Queues the passed message as a confirmation message with title.
    /// </summary>
    /// <param name="flashMessage"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public static void Confirmation(this IFlashMessage flashMessage, string message, string? title = null)
    {
        flashMessage.Queue(FlashMessageType.Confirmation, message, title);
    }
}
