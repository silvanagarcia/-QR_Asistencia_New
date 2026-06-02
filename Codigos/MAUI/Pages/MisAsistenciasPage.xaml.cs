using QRAsistencia.MAUI.Services;

namespace QRAsistencia.MAUI.Pages;

public partial class MisAsistenciasPage : ContentPage
{
    private readonly ApiService _api;

    public MisAsistenciasPage()
    {
        InitializeComponent();
        _api = Handler?.MauiContext?.Services.GetService<ApiService>()
               ?? new ApiService(new HttpClient { BaseAddress = new Uri("http://77.81.230.76:5095/") });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAsistenciasAsync();
    }

    /// <summary>
    /// Equivalente al método get() + doGetRequest() + onPostExecute() de PedirAsistenciaActivity.java
    /// </summary>
    private async Task CargarAsistenciasAsync()
    {
        // Verificar conectividad (equivalente al ConnectivityManager de Android)
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Sin conexión", "No hay conexión a internet", "OK");
            return;
        }

        Loader.IsRunning = true;
        Loader.IsVisible = true;
        BtnVolver.IsEnabled = false;

        try
        {
            int dni = Preferences.Get("DNI", -1);
            var asistencias = await _api.ObtenerAsistenciasAsync(dni);
            ListaAsistencias.ItemsSource = asistencias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al pedir las asistencias: {ex.Message}", "OK");
        }
        finally
        {
            Loader.IsRunning = false;
            Loader.IsVisible = false;
            BtnVolver.IsEnabled = true;
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
