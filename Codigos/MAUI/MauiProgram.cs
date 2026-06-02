using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace QRAsistencia.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader(); // ZXing.Net.MAUI

        builder.Services.AddSingleton<HttpClient>(sp =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://77.81.230.76:5095/")
            };
            return client;
        });

        builder.Services.AddSingleton<Services.ApiService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
