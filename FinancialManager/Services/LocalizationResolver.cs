using FinancialManager.Helpers;
using FinancialManager.Models;

namespace FinancialManager.Services;

/// <summary>
/// Immutable snapshot of localizations indexed by parent id and language code,
/// used to apply display names to entities without repeated list scans.
/// </summary>
public sealed class LocalizationResolver
{
    private readonly Dictionary<Guid, Dictionary<string, string>> _valuesByParent;
    private readonly string _currentLanguage;

    public LocalizationResolver(IEnumerable<Localization> localizations, string currentLanguage)
    {
        ArgumentNullException.ThrowIfNull(localizations);

        _currentLanguage = currentLanguage;
        _valuesByParent = new Dictionary<Guid, Dictionary<string, string>>();

        foreach (var localization in localizations)
        {
            if (!_valuesByParent.TryGetValue(localization.ParentId, out var byLanguage))
            {
                byLanguage = new Dictionary<string, string>();
                _valuesByParent[localization.ParentId] = byLanguage;
            }

            // Keep the first value per language to match the previous FirstOrDefault behaviour
            // (features store name + description under the same parent id and language).
            if (!byLanguage.ContainsKey(localization.LanguageCode))
            {
                byLanguage[localization.LanguageCode] = localization.Value;
            }
        }
    }

    public string Resolve(Guid parentId, string fallback)
    {
        if (_valuesByParent.TryGetValue(parentId, out var byLanguage))
        {
            if (byLanguage.TryGetValue(_currentLanguage, out var currentValue) && !string.IsNullOrEmpty(currentValue))
            {
                return currentValue;
            }

            if (byLanguage.TryGetValue(StaticData.EnCode, out var englishValue) && !string.IsNullOrEmpty(englishValue))
            {
                return englishValue;
            }
        }

        return fallback;
    }

    public void Apply(IEnumerable<ILocalizable> entities, string fallback)
    {
        foreach (var entity in entities)
        {
            entity.LocalizedName = Resolve(entity.Id, fallback);
        }
    }

    public void ApplyToTransactions(IEnumerable<Transaction> transactions, string categoryFallback, string typeFallback)
    {
        foreach (var transaction in transactions)
        {
            if (transaction.Category != null)
            {
                transaction.Category.LocalizedName = Resolve(transaction.Category.Id, categoryFallback);
            }

            if (transaction.TransactionType != null)
            {
                transaction.TransactionType.LocalizedName = Resolve(transaction.TransactionType.Id, typeFallback);
            }
        }
    }
}
