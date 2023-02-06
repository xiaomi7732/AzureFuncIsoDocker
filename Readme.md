# Repro

## Steps

1. Replace the instrumentation key and AzureWebJobsStorage conn string with appropriate values\
2. Build the k8senricherissue.csproj
3. Open command prompt and navigate to the path: k8senricherissue\bin\Debug\net6.0
4. Run 
    ```shell
    docker build . -t k8senricherissue2
    kubectl delete -f deploy.yaml
    kubectl apply -f deploy.yaml
    ```
5. Expose port 80 of the pod: 
   1. kubectl expose pod $PodName --type=LoadBalancer --port=80
   2. To test the application go to http://localhost/api/Function1, 
   3. to see the healthcheck go to http://localhost
6. To redeploy after making any changes run line below and repeat the steps from 2 to 7
    ```shell
    kubectl delete -f deploy.yaml
    ```

## Configurations

1. Add NuGet package (currently in preview)

    ```xml
    <PackageReference Include="Microsoft.Azure.Functions.Worker.ApplicationInsights" Version="1.0.0-preview3" />
    ```

2. Add required services, refer to [Program.cs](./Program.cs) for an example.

    ```csharp
    IHostBuilder host = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults(builder =>
        {
            // Enable application insights for workers in function. Refer to https://github.com/Azure/azure-functions-dotnet-worker/pull/944#issue-1282987627 for more details.
            builder.AddApplicationInsights().AddApplicationInsightsLogger();
        })
        .ConfigureServices(services =>
        {
            // Enable application insights for K8s in worker.
            services.AddApplicationInsightsKubernetesEnricher(null, Microsoft.Extensions.Logging.LogLevel.Information);
        });

    IHost app = host.Build();
    app.Run();
    ```

3. Manually instrument the requests

    ```csharp
    // Manually created a request that will use the telemetry initializer
    using (IOperationHolder<RequestTelemetry> operation = _telemetryClient.StartOperation<RequestTelemetry>("Func1Request"))
    {
        try
        {
            // Warning will be send to application insights.
            _logger.LogWarning("This is a warning!");

            // ... Your other code
        }
        catch
        {
            operation.Telemetry.Success = false;
            throw;
        }
    }
    ```
    Refer to [Func1.cs](./Func1.cs) for an example.

Why: 

> However, note that logs emitted by Functions include those from two processes: the Functions host and the isolated worker. And any customizations added to the worker will only apply to the worker and not to those logs emitted by the host.

Refer to <https://github.com/Azure/azure-functions-dotnet-worker/issues/1312> for more details.
