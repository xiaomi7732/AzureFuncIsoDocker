using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Kubernetes;

namespace AzFuncIsoDocker
{
    public class Program
    {
        public static void Main()
        {
            IHostBuilder host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(builder =>
                {
                    builder.AddApplicationInsights()
                           .AddApplicationInsightsLogger();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ITelemetryInitializer, MyInit>();
                    services.AddApplicationInsightsKubernetesEnricher(null, Microsoft.Extensions.Logging.LogLevel.Information);
                });
            IHost app = host.Build();
            app.Run();
        }
    }
}