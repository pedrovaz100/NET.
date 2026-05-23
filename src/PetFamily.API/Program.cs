using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetFamily.API.Middleware;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Services;
using PetFamily.Infrastructure.Data;
using PetFamily.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PetFamily API",
        Version = "v1",
        Description = "API RESTful para gerenciamento veterinário — Tutores, Pets, Consultas e Lembretes.",
        Contact = new OpenApiContact
        {
            Name = "Pedro Vaz",
            Email = "rm566551@fiap.com.br"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<ILembreteRepository, LembreteRepository>();

builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<ILembreteService, LembreteService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrations. A API continuará iniciando normalmente.");
    }
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetFamily API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
