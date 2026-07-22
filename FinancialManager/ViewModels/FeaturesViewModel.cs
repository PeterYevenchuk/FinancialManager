using CommunityToolkit.Mvvm.ComponentModel;
using FinancialManager.Data.Contracts;
using FinancialManager.Models;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class FeaturesViewModel : ObservableObject
{
    private readonly IFeatureRepository _featureRepository;

    [ObservableProperty]
    private ObservableCollection<Feature> features = new();

    public FeaturesViewModel(IFeatureRepository featureRepository)
    {
        _featureRepository = featureRepository;
    }

    public async Task InitializeAsync()
    {
        var list = await _featureRepository.GetFeaturesAsync();

        foreach (var feature in list)
        {
            feature.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(Feature.IsEnabled))
                {
                    await _featureRepository.UpdateFeatureAsync(feature);
                }
            };
        }

        Features = new ObservableCollection<Feature>(list);
    }
}
