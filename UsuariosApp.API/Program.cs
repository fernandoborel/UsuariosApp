using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using UsuariosApp.API.Components;
using UsuariosApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// injeção de dependência
builder.Services.AddScoped(map => new UsuarioRepository(builder.Configuration.GetConnectionString("UsuariosApp")));

// Configuração do JWT
var jwtSettings = new JwtSettings();
new ConfigureFromConfigurationOptions<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")).Configure(jwtSettings);

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<JwtBearerComponent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Scalar
app.MapScalarApiReference(opt => opt.WithTheme(ScalarTheme.BluePlanet));

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }