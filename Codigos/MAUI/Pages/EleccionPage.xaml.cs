using QRAsistencia.MAUI.Services;
using ZXing.Net.Maui;

namespace QRAsistencia.MAUI.Pages;

public partial class EleccionPage : ContentPage
{
    private readonly ApiService _api;
    private bool _procesando = false;

    public EleccionPage()
    {
        InitializeComponent();
        _api = Handler?.MauiContext?.Services.GetService<ApiService>()
               ?? new ApiService(new HttpClient { BaseAddress = new Uri("http://77.81.230.76:5095/") });

        // Configurar el lector QR
        BarcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            TryHarder = true
        };
    }

    // -------------------------------------------------------------------------
    // Botón: Tomar Asistencia → abrir escáner QR
    // -------------------------------------------------------------------------
    private async void OnTomarClicked(object sender, EventArgs e)
    {
        // Pedir permiso de cámara
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso denegado", "Se necesita acceso a la cámara para escanear el QR", "OK");
            return;
        }

        // Mostrar el escáner y ocultar los botones
        MainView.IsVisible = false;
        BarcodeReader.IsVisible = true;
        BarcodeReader.IsDetecting = true;
        _procesando = false;
    }

    // -------------------------------------------------------------------------
    // Callback cuando ZXing detecta un código de barras
    // Equivalente a onSuccess(List<Barcode>) de EleccionActivity.java
    // -------------------------------------------------------------------------
    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_procesando) return;
        _procesando = true;

        // Detener la cámara antes de procesar
        BarcodeReader.IsDetecting = false;

        var qrLeido = e.Results.FirstOrDefault()?.Value;
        if (string.IsNullOrEmpty(qrLeido))
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error", "QR no leído", "OK");
                VolverABotones();
            });
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                string mac = Preferences.Get("IdAndroid", "");
                string? resultado = await _api.TomarAsistenciaAsync(mac, qrLeido);
                await DisplayAlert("Asistencia", resultado ?? "Registrado", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al tomar asistencia: {ex.Message}", "OK");
            }
            finally
            {
                VolverABotones();
            }
        });
    }

    private void VolverABotones()
    {
        BarcodeReader.IsVisible = false;
        BarcodeReader.IsDetecting = false;
        MainView.IsVisible = true;
    }

    // -------------------------------------------------------------------------
    // Botón: Mis Asistencias → navegar a PedirAsistenciaActivity
    // -------------------------------------------------------------------------
    private async void OnMisAsistenciasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MisAsistenciasPage());
    }

    protected override bool OnBackButtonPressed()
    {
        // Si el escáner está abierto, ciérralo; si no, comportamiento normal
        if (BarcodeReader.IsVisible)
        {
            VolverABotones();
            return true;
        }
        return base.OnBackButtonPressed();
    }
}
