using FlashMessage.Contracts;
using FlashMessage.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FlashMessage.Core;

public class FlashMessage(
    ITempDataDictionaryFactory tempDataFactory,
    IHttpContextAccessor httpContextAccessor,
    IFlashMessageSerializer messageSerializer
) : IFlashMessage
{
    private static string KeyName { get; set; } = "_FlashMessage";


    private ITempDataDictionary? _tempData;

    public ITempDataDictionary TempData
    {
        protected set => _tempData = value;
        get
        {
            if (_tempData is null)
            {
                _tempData = tempDataFactory.GetTempData(httpContextAccessor.HttpContext);
            }

            return _tempData;
        }
    }


    /// <summary>
    /// Queues the passed flash message for display.
    /// </summary>
    /// <param name="message"></param>
    public void Queue(FlashMessageViewModel message)
    {
        // Retrieve the currently queued message.
        var messages = Peek();

        // Append the new message.
        messages.Add(message);

        // Store the messages.
        Store(messages);
    }

    /// <summary>
    /// Queues the passed flash messages for display, replacing any queued messages.
    /// </summary>
    /// <param name="messages"></param>
    private void Store(IList<FlashMessageViewModel> messages)
    {
        // Serialize the messages.
        var data = messageSerializer.Serialize(messages);

        // Set the data without doing any further securing or transformations.
        TempData[KeyName] = data;
    }

    /// <summary>
    /// Retrieves the queued flash messages for display and clears them.
    /// </summary>
    /// <returns></returns>
    public List<FlashMessageViewModel> Retrieve()
    {
        // Retrieve the data from the session store, guard for cases where it does not exist.
        var data = TempData[KeyName];
        if (data is null)
        {
            return [];
        }

        // Clear the data and return.
        TempData.Remove(KeyName);
        return messageSerializer.Deserialize((string)data);
    }

    /// <summary>
    /// Retrieves the queued messages for the current response without clearing them.
    /// </summary>
    /// <returns></returns>
    public List<FlashMessageViewModel> Peek()
    {
        // Retrieve the data from the session store, guard for cases where it does not exist.
        var data = TempData.Peek(KeyName);
        if (data is null)
        {
            return [];
        }

        // Deserialize messages and return them.
        return messageSerializer.Deserialize((string)data);
    }
}
