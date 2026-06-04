using Asistencia.Application;
using Asistencia.Application.Interfaces;
using Asistencia.Infrastructure.Data;
using Asistencia.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<IAlumnoServicio, AlumnoServicio>();
builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();

builder.Services.AddScoped<IAsistenciaServicio, AsistenciaServicio>();
builder.Services.AddScoped<IAsistenciaRepository, RegistroAsistenciaRepository>();

builder.Services.AddScoped<IControlQRServicio, ControlQRServicio>();
builder.Services.AddScoped<IControlQRRepository, ControlQRRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy("corsapp", policy =>
{
    policy.WithOrigins("*")
          .AllowAnyMethod()
          .AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"Error interno del servidor.\"}");
    });
});

app.UseCors("corsapp");
app.UseAuthorization();
app.MapControllers();
app.Run();
