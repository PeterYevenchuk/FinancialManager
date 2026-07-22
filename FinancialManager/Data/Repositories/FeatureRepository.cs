using FinancialManager.Data.Contracts;
using FinancialManager.Models;
using FinancialManager.Services.Contracts;
using SQLite;

namespace FinancialManager.Data.Repositories;

public class FeatureRepository : IFeatureRepository
{
    private readonly SQLiteAsyncConnection _connection;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    public FeatureRepository(
        SQLiteAsyncConnection connection,
        ILocalizationRepository localizationRepository,
        ILocalizationService localizationService)
    {
        _connection = connection;
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;
    }

    public async Task<List<Feature>> GetFeaturesAsync()
    {
        var features = await _connection.Table<Feature>().ToListAsync();
        var localizations = await _localizationRepository.GetAsync();
        string currentLang = _localizationService.CurrentLanguage;

        foreach (var feature in features)
        {
            var nameLoc = localizations.FirstOrDefault(l => l.ParentId == feature.Id && l.LanguageCode == currentLang)
                       ?? localizations.FirstOrDefault(l => l.ParentId == feature.Id && l.LanguageCode == "en");
            feature.LocalizedName = nameLoc?.Value ?? feature.Key;

            var descLoc = localizations.FirstOrDefault(l => l.ParentId == feature.Id && l.LanguageCode == currentLang)
                       ?? localizations.FirstOrDefault(l => l.ParentId == feature.Id && l.LanguageCode == "en");
            feature.LocalizedDescription = descLoc?.Value ?? string.Empty;
        }

        return features;
    }

    public async Task<bool> IsFeatureEnabledAsync(string featureKey)
    {
        var feature = await _connection.Table<Feature>().FirstOrDefaultAsync(f => f.Key == featureKey);
        return feature?.IsEnabled ?? false;
    }

    public async Task UpdateFeatureAsync(Feature feature)
    {
        await _connection.UpdateAsync(feature);
    }
}
