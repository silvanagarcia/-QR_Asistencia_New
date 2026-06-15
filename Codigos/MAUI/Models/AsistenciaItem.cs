namespace QRAsistencia.MAUI.Models;

public class AsistenciaItem
{
    public long Fecha { get; set; }

    private DateTimeOffset FechaLocal =>
        DateTimeOffset.FromUnixTimeSeconds(Fecha).ToLocalTime();

    // Número del día: "28"
    public string Dia => FechaLocal.ToString("dd");

    // Mes y año: "OCT 2024"
    public string MesAnio => FechaLocal.ToString("MMM yyyy").ToUpper();

    // Día de la semana: "Lunes"
    public string DiaSemana => FechaLocal.ToString("dddd");

    // Hora: "21:30 hs"
    public string Hora => FechaLocal.ToString("HH:mm") + " hs";

    // Fecha completa para accesibilidad
    public string FechaFormateada =>
        FechaLocal.ToString("dd/MM/yyyy HH:mm");
}
