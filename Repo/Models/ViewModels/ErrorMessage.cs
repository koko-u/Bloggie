namespace Bloggie.Repo.Models.ViewModels;

public readonly record struct ErrorMessage(string Message)
{
    public void Deconstruct(out string message)
    {
        message = Message;
    }
}
