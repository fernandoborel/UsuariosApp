using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UsuariosApp.API.Extensions;

public static class SwaggerDocExtension
{
    public static IServiceCollection AddSwaggerDocConfig(this IServiceCollection services)
    {
        services.AddRouting(map => map.LowercaseUrls = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "API Usuarios (v1)", Version = "v1" });
            opt.SwaggerDoc("v2", new OpenApiInfo { Title = "API Usuarios (v2)", Version = "v2" });

            opt.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;
                var groupName = apiDesc.GroupName ?? ""; 
                return groupName == docName;
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Usuarios API v1");
            opt.SwaggerEndpoint("/swagger/v2/swagger.json", "Usuarios API v2");
        });

        return app;
    }
}