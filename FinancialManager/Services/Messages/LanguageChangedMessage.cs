using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FinancialManager.Services.Messages;

public class LanguageChangedMessage : ValueChangedMessage<string>
{
    public LanguageChangedMessage(string value) : base(value) { }
}
