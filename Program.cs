using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzFuncIsoDocker
{
    public class Program
    {
        public static void Main()
        {
            IHostBuilder host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();    // Enable Application Insights for workers.
                    services.AddApplicationInsightsKubernetesEnricher(null, Microsoft.Extensions.Logging.LogLevel.Trace);        // Enable Application Insights Kubernetes to enhance telemetries.
                });
            IHost app = host.Build();
            app.Run();
        }
    }
}