namespace FlashMessage.ViewModels;

public class FlashMessageViewModel
{
    /// <summary>
    /// Gets / sets if the message contains raw HTML. If set to true, the title will not be rendered and the contents of the message parameter will
    /// be written directly to the output.
    /// </summary>
    public bool IsHtml { get; set; } = false;

    public string? Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public FlashMessageType Type { get; set; } = FlashMessageType.Info;
}
