using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using AudioPlayer_v1.Views;

namespace AudioPlayer_v1 {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<RecorderScreen>();
            builder.Services.AddTransient<Listado>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}