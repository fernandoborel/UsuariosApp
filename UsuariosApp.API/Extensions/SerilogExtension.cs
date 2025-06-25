using Serilog;
using Serilog.Events;

namespace UsuariosApp.API.Extensions;

public static class SerilogExtension
{
    public static ConfigureHostBuilder AddSerilogConfig(this ConfigureHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:80")
            .CreateLogger();

        host.UseSerilog();

        return host;
    }
}