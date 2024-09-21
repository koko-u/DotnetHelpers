using FlashMessage.ViewModels;

namespace FlashMessage.Contracts;

public interface IFlashMessage
{
    /// <summary>
    /// Queues a flash message for display with the specified message.
    /// </summary>
    /// <param name="flashMessage"></param>
    void Queue(FlashMessageViewModel flashMessage);

    /// <summary>
    /// Retrieves the queued flash messages for display and clears them.
    /// </summary>
    /// <returns></returns>
    List<FlashMessageViewModel> Retrieve();

    /// <summary>
    /// Retrieves the queued messages for the current response without clearing them.
    /// </summary>
    /// <returns></returns>
    List<FlashMessageViewModel> Peek();
}
