using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Bloggie.Web.FlashMessages;

/// <summary>
/// Flash message levels for displaying messages to the user.
/// </summary>
[JsonConverter(typeof(SmartEnumNameConverter<FlashMessageLevel, int>))]
public sealed class FlashMessageLevel : SmartEnum<FlashMessageLevel>
{
    public static readonly FlashMessageLevel Success = new FlashMessageLevel("Success", 1);
    public static readonly FlashMessageLevel Info = new FlashMessageLevel("Info", 2);
    public static readonly FlashMessageLevel Warning = new FlashMessageLevel("Warning", 3);
    public static readonly FlashMessageLevel Error = new FlashMessageLevel("Error", 4);

    private FlashMessageLevel(string name, int value)
        : base(name, value) { }
}
