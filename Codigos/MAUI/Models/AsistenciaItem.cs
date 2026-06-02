namespace QRAsistencia.MAUI.Models;

public class AsistenciaItem
{
    public long Fecha { get; set; }

    public string FechaFormateada =>
        DateTimeOffset.FromUnixTimeSeconds(Fecha)
                      .ToLocalTime()
                      .ToString("yyyy-MM-dd HH:mm:ss");
}
