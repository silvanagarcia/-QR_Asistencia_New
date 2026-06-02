using System.Text;
using System.Text.Json;
using QRAsistencia.MAUI.Models;

namespace QRAsistencia.MAUI.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// PUT /api/Alumno/mac — registra el dispositivo con el DNI del alumno.
    /// Equivalente al método Put() + hacerPut() de MainActivity.java
    /// </summary>
    public async Task<bool> RegistrarDispositivoAsync(int dni, string mac)
    {
        var json = JsonSerializer.Serialize(new { dni, mac });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PutAsync("api/Alumno/mac", content);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// POST /api/RegistroAsistencia — envía la MAC y el código QR escaneado.
    /// Equivalente al método post() + hacerPost() de EleccionActivity.java
    /// </summary>
    public async Task<string?> TomarAsistenciaAsync(string mac, string codigoQR)
    {
        var json = JsonSerializer.Serialize(new { mac, codigoQR });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("api/RegistroAsistencia", content);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error {(int)response.StatusCode}: {body}");

        return body;
    }

    /// <summary>
    /// GET /api/RegistroAsistencia/{dni} — obtiene la lista de asistencias del alumno.
    /// Equivalente al método get() + doGetRequest() de PedirAsistenciaActivity.java
    /// </summary>
    public async Task<List<AsistenciaItem>> ObtenerAsistenciasAsync(int dni)
    {
        var response = await _http.GetAsync($"api/RegistroAsistencia/{dni}");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var items = JsonSerializer.Deserialize<List<AsistenciaItem>>(body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return items ?? new List<AsistenciaItem>();
    }
}
