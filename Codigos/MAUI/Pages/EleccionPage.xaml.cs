using QRAsistencia.MAUI.Services;
using ZXing.Net.Maui;

namespace QRAsistencia.MAUI.Pages;

public partial class EleccionPage : ContentPage
{
    private readonly ApiService _api;
    private bool _procesando = false;
    private bool _linternaActiva = false;
    private CancellationTokenSource? _animCts;

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
    // Botón: Cancelar
    // ─────────────────────────────────────────────────────────────────────────
    private void OnCancelarClicked(object sender, EventArgs e) => VolverABotones();

    // ─────────────────────────────────────────────────────────────────────────
    // Botón: Linterna
    // ─────────────────────────────────────────────────────────────────────────
    private void OnLinternaClicked(object sender, TappedEventArgs e)
    {
        _linternaActiva = !_linternaActiva;
        BarcodeReader.IsTorchOn = _linternaActiva;
        LblLinterna.Text = _linternaActiva ? "💡" : "🔦";
        BtnLinterna.BackgroundColor = _linternaActiva
            ? Color.FromArgb("#3B82F6")
            : Color.FromArgb("#1E293B");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Callback ZXing — código detectado
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
                MostrarEstado("No se pudo leer, intentá de nuevo...");
                await Task.Delay(1500);
                ReiniciarEscaner();
                return;
            }

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

                if (reintentar) ReiniciarEscaner();
                else VolverABotones();
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
    // Navegación: Mis Asistencias
    // ─────────────────────────────────────────────────────────────────────────
    private async void OnMisAsistenciasClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new MisAsistenciasPage());

    // ─────────────────────────────────────────────────────────────────────────
    // Botón físico atrás
    // ─────────────────────────────────────────────────────────────────────────
    protected override bool OnBackButtonPressed()
    {
        if (ScannerView.IsVisible) { VolverABotones(); return true; }
        return base.OnBackButtonPressed();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────
    private void AbrirEscaner()
    {
        _procesando = false;
        _linternaActiva = false;
        LblLinterna.Text = "🔦";
        BtnLinterna.BackgroundColor = Color.FromArgb("#1E293B");
        MostrarEstado("Buscando QR...");
        MostrarProcesando(false);
        MainView.IsVisible = false;
        ScannerView.IsVisible = true;
        BarcodeReader.IsDetecting = true;
        IniciarAnimacionLinea();
    }

    private void ReiniciarEscaner()
    {
        _procesando = false;
        MostrarEstado("Buscando QR...");
        MostrarProcesando(false);
        BarcodeReader.IsDetecting = true;
    }

    private void VolverABotones()
    {
        DetenerAnimacionLinea();
        BarcodeReader.IsDetecting = false;
        BarcodeReader.IsTorchOn = false;
        _linternaActiva = false;
        ScannerView.IsVisible = false;
        MainView.IsVisible = true;
        _procesando = false;
    }

    private void MostrarEstado(string msg) => LblEstado.Text = msg;

    private void MostrarProcesando(bool activo)
    {
        ScanLoader.IsRunning = activo;
        ScanLoader.IsVisible = activo;
        LineaEscaneo.IsVisible = !activo;
        LblEstado.Text = activo ? "Registrando asistencia..." : "Buscando QR...";
    }

    // ─── Animación de la línea de escaneo ────────────────────────────────────
    private void IniciarAnimacionLinea()
    {
        _animCts = new CancellationTokenSource();
        var token = _animCts.Token;

        Task.Run(async () =>
        {
            // Recorre desde y=0.37 hasta y=0.67 y vuelve
            double[] posiciones = { 0.37, 0.42, 0.47, 0.52, 0.57, 0.62, 0.67,
                                    0.62, 0.57, 0.52, 0.47, 0.42, 0.37 };
            int i = 0;
            while (!token.IsCancellationRequested)
            {
                var pos = posiciones[i % posiciones.Length];
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    AbsoluteLayout.SetLayoutBounds(LineaEscaneo,
                        new Rect(0.08, pos, 0.84, 0.002));
                });
                i++;
                await Task.Delay(80, token).ContinueWith(_ => { });
            }
        }, token);
    }

    private void DetenerAnimacionLinea()
    {
        _animCts?.Cancel();
        _animCts = null;
    }
}
