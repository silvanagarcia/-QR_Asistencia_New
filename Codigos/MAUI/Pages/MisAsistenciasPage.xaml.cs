using QRAsistencia.MAUI.Services;

namespace QRAsistencia.MAUI.Pages;

public partial class MisAsistenciasPage : ContentPage
{
    private readonly ApiService _api;

    public MisAsistenciasPage()
    {
        InitializeComponent();
        _api = Handler?.MauiContext?.Services.GetService<ApiService>()
               ?? new ApiService(new HttpClient
               {
                   BaseAddress = new Uri("http://77.81.230.76:5095/"),
                   Timeout = TimeSpan.FromSeconds(15)
               });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAsistenciasAsync();
    }

    private async Task CargarAsistenciasAsync()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlertAsync("Sin conexión", "No hay conexión a internet.", "OK");
            return;
        }

        Loader.IsRunning = true;
        Loader.IsVisible = true;
        BtnVolver.IsEnabled = false;
        BadgeTotal.IsVisible = false;

        try
        {
            int dni = Preferences.Get("DNI", -1);
            var asistencias = await _api.ObtenerAsistenciasAsync(dni);

            // Ordenar de más reciente a más antiguo
            var ordenadas = asistencias.OrderByDescending(a => a.Fecha).ToList();
            ListaAsistencias.ItemsSource = ordenadas;

            // Mostrar total
            if (ordenadas.Count > 0)
            {
                LblTotal.Text = ordenadas.Count.ToString();
                BadgeTotal.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudieron cargar las asistencias: {ex.Message}", "OK");
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
