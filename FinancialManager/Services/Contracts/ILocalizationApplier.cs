using FinancialManager.Services;

namespace FinancialManager.Services.Contracts;

/// <summary>
/// Loads the current localization snapshot so callers can resolve display names
/// without repeating the Localization lookup logic in every view model.
/// </summary>
public interface ILocalizationApplier
{
    Task<LocalizationResolver> CreateResolverAsync();
}
