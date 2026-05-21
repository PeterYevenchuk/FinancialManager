using FinancialManager.Data.Repositories;
using FinancialManager.Services;
using FinancialManager.ViewModels;
using FinancialManager.Views;
using Microsoft.Extensions.Logging;

namespace FinancialManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<ITransactionTypeRepository, TransactionTypeRepository>();
            builder.Services.AddSingleton<ILocalizationRepository, LocalizationRepository>();

            builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
            builder.Services.AddSingleton<ILocalizationManager, LocalizationManager>();

            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<CategoryAddViewModel>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<TransactionTypeAddViewModel>();
            builder.Services.AddTransient<TransactionTypeViewModel>();
            builder.Services.AddTransient<TransactionViewModel>();
            builder.Services.AddTransient<TransactionAddViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddTransient<TransactionAddPage>();
            builder.Services.AddTransient<TransactionPage>();
            builder.Services.AddTransient<TransactionTypeAddPage>();
            builder.Services.AddTransient<TransactionTypePage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<CategoryAddPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
