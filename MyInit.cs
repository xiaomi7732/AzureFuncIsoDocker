using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.Kubernetes;

internal class MyInit : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if(telemetry is ISupportProperties withProperties)
        {
            withProperties.Properties["K8sTest"] = "Pass";
        }
    }
}