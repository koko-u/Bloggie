using System.Text.Json.Serialization;

namespace Bloggie.Web.Models.ViewModels;

public class Notification
{
    public  string Message { get;  }

    public  string NotificationType { get;  }

    [JsonConstructor]
    private Notification(string message, string notificationType)
    {
        Message = message;
        NotificationType = notificationType;
    }

    public static Notification Success(string message) => new(message, "alert-success");

    public static Notification Info(string message) => new(message, "alert-info");

    public static Notification Error(string message) => new(message, "alert-danger");
}
