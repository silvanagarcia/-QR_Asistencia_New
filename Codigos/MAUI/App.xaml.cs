namespace QRAsistencia.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Si ya está logueado, ir directo a la pantalla de elección
        bool isLoggedIn = Preferences.Get("isLoggedIn", false);
        MainPage = isLoggedIn
            ? new NavigationPage(new Pages.EleccionPage())
            : new NavigationPage(new Pages.LoginPage());
    }
}
