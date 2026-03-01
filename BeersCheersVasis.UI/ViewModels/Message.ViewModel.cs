using System.Collections.Immutable;

namespace BeersCheersVasis.UI.ViewModels;
public record MessageViewModel
{
    public ImmutableList<string> ErrorMessages { get; init; } = ImmutableList<string>.Empty;
    public ImmutableList<string> SuccessMessages { get; init; } = ImmutableList<string>.Empty;

    public MessageViewModel ClearErrors()
    {
        return this with
        {
            ErrorMessages = ImmutableList<string>.Empty
        };
    }

    public MessageViewModel SetErrorMessages(string success)
    {
        return this with
        {
            ErrorMessages = ErrorMessages.AddRange(new List<string>() { success })
        };
    }

    public MessageViewModel SetErrorMessages(List<string> successes)
    {
        return this with
        {
            ErrorMessages = ErrorMessages.AddRange(successes)
        };
    }

    public MessageViewModel SetSuccessMessages(string success)
    {
        return this with
        {
            SuccessMessages = SuccessMessages.AddRange(new List<string>() { success })
        };
    }

    public MessageViewModel SetSuccessMessages(List<string> successes)
    {
        return this with
        {
            SuccessMessages = SuccessMessages.AddRange(successes)
        };
    }

    public MessageViewModel ClearSuccessMessages()
    {
        return this with
        {
            SuccessMessages = ImmutableList<string>.Empty
        };
    }
}