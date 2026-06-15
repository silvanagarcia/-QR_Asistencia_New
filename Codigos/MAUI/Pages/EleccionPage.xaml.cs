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
               ?? new ApiService(new HttpClient
               {
                   BaseAddress = new Uri("http://77.81.230.76:5095/"),
                   Timeout = TimeSpan.FromSeconds(15)
               });

        BarcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            TryHarder = true,
            Multiple = false
        };
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Botón: Escanear QR
    // ─────────────────────────────────────────────────────────────────────────
    private async void OnTomarClicked(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlertAsync("Permiso denegado",
                "Se necesita acceso a la cámara para escanear el QR.", "OK");
            return;
        }

        AbrirEscaner();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Botón: Cancelar (dentro del escáner)
    // ─────────────────────────────────────────────────────────────────────────
    private void OnCancelarClicked(object sender, EventArgs e)
    {
        VolverABotones();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Callback de ZXing cuando detecta un código
    // ─────────────────────────────────────────────────────────────────────────
    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_procesando) return;
        _procesando = true;

        BarcodeReader.IsDetecting = false;

        var qrLeido = e.Results.FirstOrDefault()?.Value;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (string.IsNullOrEmpty(qrLeido))
            {
                MostrarEstado("No se pudo leer el QR, intentá de nuevo...");
                await Task.Delay(1500);
                ReiniciarEscaner();
                return;
            }

            // Mostrar indicador de carga
            MostrarProcesando(true);

            try
            {
                string mac = Preferences.Get("IdAndroid", "");
                string? resultado = await _api.TomarAsistenciaAsync(mac, qrLeido);

                MostrarProcesando(false);
                await DisplayAlertAsync("✅ Asistencia registrada",
                    resultado ?? "Tu asistencia fue registrada correctamente.", "OK");
                VolverABotones();
            }
            catch (HttpRequestException ex)
            {
                MostrarProcesando(false);
                bool reintentar = await DisplayAlert("Error al registrar",
                    $"No se pudo registrar la asistencia.\n{ex.Message}",
                    "Reintentar", "Cancelar");

                if (reintentar)
                    ReiniciarEscaner();
                else
                    VolverABotones();
            }
            catch (Exception ex)
            {
                MostrarProcesando(false);
                await DisplayAlertAsync("Error", ex.Message, "OK");
                VolverABotones();
            }
        });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Navegación
    // ─────────────────────────────────────────────────────────────────────────
    private async void OnMisAsistenciasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MisAsistenciasPage());
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Botón físico atrás: si el escáner está abierto, lo cierra
    // ─────────────────────────────────────────────────────────────────────────
    protected override bool OnBackButtonPressed()
    {
        if (ScannerView.IsVisible)
        {
            VolverABotones();
            return true;
        }
        return base.OnBackButtonPressed();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers de estado
    // ─────────────────────────────────────────────────────────────────────────
    private void AbrirEscaner()
    {
        _procesando = false;
        MostrarEstado("Buscando código QR...");
        MostrarProcesando(false);
        MainView.IsVisible = false;
        ScannerView.IsVisible = true;
        BarcodeReader.IsDetecting = true;
    }

    private void ReiniciarEscaner()
    {
        _procesando = false;
        MostrarEstado("Buscando código QR...");
        MostrarProcesando(false);
        BarcodeReader.IsDetecting = true;
    }

    private void VolverABotones()
    {
        BarcodeReader.IsDetecting = false;
        ScannerView.IsVisible = false;
        MainView.IsVisible = true;
        _procesando = false;
    }

    private void MostrarEstado(string mensaje)
    {
        LblEstado.Text = mensaje;
    }

    private void MostrarProcesando(bool procesando)
    {
        ScanLoader.IsRunning = procesando;
        ScanLoader.IsVisible = procesando;
        MarcoQR.IsVisible = !procesando;
        LblEstado.Text = procesando ? "Registrando asistencia..." : "Buscando código QR...";
    }
}
