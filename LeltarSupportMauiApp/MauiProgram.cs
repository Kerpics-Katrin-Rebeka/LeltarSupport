using LeltarSupportMauiApp.ViewModels;
using LeltarSupportMauiApp.Views;
using Microsoft.Extensions.Logging;

namespace LeltarSupportMauiApp
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
            builder.Services.AddSingleton<ProductListView>();
            builder.Services.AddSingleton<ProductListViewModel>();
            builder.Services.AddSingleton<ProductDetailsView>();
            builder.Services.AddSingleton<ProductDetailsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
