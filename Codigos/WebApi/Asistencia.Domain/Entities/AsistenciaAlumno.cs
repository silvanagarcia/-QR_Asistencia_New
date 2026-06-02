using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Asistencia.Domain.Entities;

[Table("registro_asistencia")]
public class AsistenciaAlumno
{
    [Key]
    [Column("registro_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdRegistro { get; set; }
    
    [Column("fecha")]
    public long Fecha { get; set; }

    [Column("dni")]
    [ForeignKey("Alumno")]
    public int AlumnoDNI { get; set; }

    public Alumno Alumno { get; set; }
}
