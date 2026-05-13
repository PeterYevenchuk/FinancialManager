using FinancialManager.Data.Repositories;
using FinancialManager.Models;
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

            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<CategoryAddPage>();
            builder.Services.AddTransient<CategoryAddViewModel>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<CategoryViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
