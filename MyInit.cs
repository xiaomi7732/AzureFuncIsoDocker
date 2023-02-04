using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace Microsoft.ApplicationInsights.Kubernetes;

internal class MyInit : ITelemetryInitializer
{
    private readonly ILogger<MyInit> logger;

    public MyInit(ILogger<MyInit> logger)
    {
        this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }
    public void Initialize(ITelemetry telemetry)
    {
        this.logger.LogInformation("Hello my info!");
        if(telemetry is ISupportProperties withProperties)
        {
            withProperties.Properties["K8sTest"] = "Pass";
        }
    }
}