using System.Text.Json;
using FlashMessage.Contracts;
using FlashMessage.ViewModels;

namespace FlashMessage.Serializers;

public class JsonFlashMessageSerializer : IFlashMessageSerializer
{
    /// <summary>
    /// Deserializes a serialized collection of flash messages.
    /// </summary>
    /// <param name="data">serializedMessages</param>
    /// <returns></returns>
    public List<FlashMessageViewModel> Deserialize(string data)
    {

        if (string.IsNullOrWhiteSpace(data))
        {
            return [];
        }

        var messages = JsonSerializer.Deserialize<List<FlashMessageViewModel>>(data);
        return messages ?? [];
    }

    /// <summary>
    /// Serializes the passed list of messages to json format.
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public string Serialize(IList<FlashMessageViewModel> messages)
    {

        var data = JsonSerializer.Serialize(messages);
        return data;
    }
}
