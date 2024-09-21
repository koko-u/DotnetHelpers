using FlashMessage.ViewModels;

namespace FlashMessage.Contracts;

public interface IFlashMessageSerializer
{
    List<FlashMessageViewModel> Deserialize(string data);

    string Serialize(IList<FlashMessageViewModel> messages);
}
