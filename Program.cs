using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Kubernetes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzFuncIsoDocker
{
    public class Program
    {
        public static void Main()
        {
            IHostBuilder host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(builder =>
                {
                    builder.Services.AddSingleton<ITelemetryInitializer, MyInit>();
                    builder.Services.AddApplicationInsightsKubernetesEnricher(null, Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .ConfigureServices(services =>{
                    services.AddSingleton<ITelemetryInitializer, MyInit>();
                });
            IHost app = host.Build();
            app.Run();
        }
    }
}