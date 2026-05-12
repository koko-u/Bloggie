using System.Text.Json;

namespace Bloggie.Web.FlashMessages;

/// <summary>
/// Flash message with level and message content.
/// </summary>
/// <param name="Level"></param>
/// <param name="Message"></param>
public record struct FlashMessage(FlashMessageLevel Level, string Message)
{
    /// <summary>
    /// TempData key for storing flash messages.
    /// </summary>
    public const string Key = "FlashMessage";

    /// <summary>
    /// Create success flash message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static FlashMessage Success(string message) => new(FlashMessageLevel.Success, message);

    /// <summary>
    /// Create informational flash message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static FlashMessage Info(string message) => new(FlashMessageLevel.Info, message);

    /// <summary>
    /// Create warning flash message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static FlashMessage Warning(string message) => new(FlashMessageLevel.Warning, message);

    /// <summary>
    /// Create error flash message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static FlashMessage Error(string message) => new(FlashMessageLevel.Error, message);

    /// <summary>
    /// Convert to string for storage in TempData.
    /// </summary>
    /// <returns></returns>
    public string ToJsonString() => JsonSerializer.Serialize(this);

    /// <summary>
    /// Restore FlashMessage from JSON string (TempData).
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static FlashMessage FromJsonString(string json) =>
        JsonSerializer.Deserialize<FlashMessage>(json);
}
