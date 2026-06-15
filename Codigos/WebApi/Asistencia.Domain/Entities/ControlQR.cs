using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asistencia.Domain.Entities;

[Table("control_qr")]
public class ControlQR
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("key")]
    public string Key { get; set; } = string.Empty;

    [Column("valor")]
    public string Valor { get; set; } = string.Empty;

    [Column("generado_en")]
    public long GeneradoEn { get; set; }
}
