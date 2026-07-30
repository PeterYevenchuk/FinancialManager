namespace FinancialManager.Models;

/// <summary>
/// Entity whose display name is resolved from the Localization table by its Id.
/// </summary>
public interface ILocalizable
{
    Guid Id { get; }

    string LocalizedName { get; set; }
}
