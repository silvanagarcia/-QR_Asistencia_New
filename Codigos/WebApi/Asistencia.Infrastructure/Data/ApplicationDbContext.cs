using Asistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asistencia.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}

    public DbSet<Alumno> Alumnos{get; set;}
    public DbSet<AsistenciaAlumno> AsistenciasR{get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
