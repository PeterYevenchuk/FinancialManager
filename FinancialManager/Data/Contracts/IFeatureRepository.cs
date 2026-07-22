using FinancialManager.Models;

namespace FinancialManager.Data.Contracts;

public interface IFeatureRepository
{
    Task<List<Feature>> GetFeaturesAsync();
    Task<bool> IsFeatureEnabledAsync(string featureKey);
    Task UpdateFeatureAsync(Feature feature);
}
