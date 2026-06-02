using QRAsistencia.MAUI.Services;

namespace QRAsistencia.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _api;

    public LoginPage()
    {
        InitializeComponent();
        _api = Handler?.MauiContext?.Services.GetService<ApiService>()
               ?? new ApiService(new HttpClient { BaseAddress = new Uri("http://77.81.230.76:5095/") });
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var dniText = EntryDNI.Text?.Trim();
        if (string.IsNullOrEmpty(dniText) || !int.TryParse(dniText, out int dni))
        {
            await DisplayAlert("Error", "Ingrese un DNI válido", "OK");
            return;
        }

        SetLoading(true);
        try
        {
            // Obtener el ID único del dispositivo (equivalente a ANDROID_ID)
            string deviceId = DeviceInfo.Current.Name + "_" + DeviceInfo.Current.Platform;
#if ANDROID
            deviceId = Android.Provider.Settings.Secure.GetString(
                Android.App.Application.Context.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId) ?? deviceId;
#endif

            bool ok = await _api.RegistrarDispositivoAsync(dni, deviceId);
            if (ok)
            {
                // Guardar sesión (equivalente a SharedPreferences)
                Preferences.Set("isLoggedIn", true);
                Preferences.Set("DNI", dni);
                Preferences.Set("IdAndroid", deviceId);

                // Navegar a la pantalla de elección
                Application.Current!.MainPage = new NavigationPage(new EleccionPage());
            }
            else
            {
                await DisplayAlert("Error", "Intentalo de nuevo", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Intentalo de nuevo: {ex.Message}", "OK");
        }
        finally
        {
            SetLoading(false);
        }
    }

    private void SetLoading(bool loading)
    {
        BtnLogin.IsEnabled = !loading;
        Loader.IsRunning = loading;
        Loader.IsVisible = loading;
    }
}
