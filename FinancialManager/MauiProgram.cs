using FinancialManager.Data;
using FinancialManager.Data.Contracts;
using FinancialManager.Data.Repositories;
using FinancialManager.Services;
using FinancialManager.Services.Contracts;
using FinancialManager.ViewModels;
using FinancialManager.Views;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SQLite;

namespace FinancialManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "FinData.db");

            builder.Services.AddSingleton(new SQLiteAsyncConnection(dbPath));
            builder.Services.AddSingleton<DatabaseInitializer>();

            builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<ITransactionTypeRepository, TransactionTypeRepository>();
            builder.Services.AddSingleton<ILocalizationRepository, LocalizationRepository>();
            builder.Services.AddSingleton<IFeatureRepository, FeatureRepository>();

            builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
            builder.Services.AddSingleton<ILocalizationManager, LocalizationManager>();
            builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
            builder.Services.AddSingleton<IExportService, ExportService>();
            builder.Services.AddSingleton<JsonBackupService>();

            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<CategoryAddViewModel>();
            builder.Services.AddTransient<TransactionTypeAddViewModel>();
            builder.Services.AddTransient<TransactionTypeViewModel>();
            builder.Services.AddTransient<TransactionViewModel>();
            builder.Services.AddTransient<TransactionAddViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddTransient<BackupViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<FeaturesViewModel>();

            builder.Services.AddTransient<TransactionAddPage>();
            builder.Services.AddTransient<TransactionPage>();
            builder.Services.AddTransient<TransactionTypeAddPage>();
            builder.Services.AddTransient<TransactionTypePage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<CategoryAddPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<BackupPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<FeaturesPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
