using FinancialManager.Data.Contracts;
using FinancialManager.Helpers;
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
            // A feature stores two localization rows per language under the same ParentId,
            // in seeding order: [0] = name, [1] = description. Resolving both from a single
            // FirstOrDefault returned the name twice, so the description showed the name.
            var localized = GetFeatureTexts(localizations, feature.Id, currentLang);
            if (localized.Count < 2)
            {
                localized = GetFeatureTexts(localizations, feature.Id, StaticData.EnCode);
            }

            feature.LocalizedName = localized.ElementAtOrDefault(0) ?? feature.Key;
            feature.LocalizedDescription = localized.ElementAtOrDefault(1) ?? string.Empty;
        }

        return features;
    }

    private static List<string> GetFeatureTexts(List<Localization> localizations, Guid featureId, string languageCode)
    {
        return localizations
            .Where(l => l.ParentId == featureId && l.LanguageCode == languageCode)
            .Select(l => l.Value)
            .ToList();
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
