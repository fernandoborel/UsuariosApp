﻿using Microsoft.Extensions.Options;
using UsuariosApp.API.Components;
using UsuariosApp.API.Repositories;

namespace UsuariosApp.API.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        //repository
        services.AddScoped(map => new UsuarioRepository(configuration.GetConnectionString("UsuariosApp")));

        //jwt
        var jwtSettings = new JwtSettings();
        new ConfigureFromConfigurationOptions<JwtSettings>(configuration.GetSection("JwtSettings")).Configure(jwtSettings);

        services.AddSingleton(jwtSettings);
        services.AddSingleton<JwtBearerComponent>();

        //RabbitMQ
        var rabbitMQSettings = new RabbitMQSettings();
        new ConfigureFromConfigurationOptions<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings")).Configure(rabbitMQSettings);

        services.AddSingleton(rabbitMQSettings);
        services.AddScoped<RabbitMQComponent>();

        return services;
    }
}