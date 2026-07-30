using FinancialManager.Data.Contracts;
using FinancialManager.Services.Contracts;

namespace FinancialManager.Services;

public sealed class LocalizationApplier : ILocalizationApplier
{
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    public LocalizationApplier(
        ILocalizationRepository localizationRepository,
        ILocalizationService localizationService)
    {
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;
    }

    public async Task<LocalizationResolver> CreateResolverAsync()
    {
        var localizations = await _localizationRepository.GetAsync();
        return new LocalizationResolver(localizations, _localizationService.CurrentLanguage);
    }
}
