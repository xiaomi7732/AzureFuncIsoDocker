using System.Collections.Generic;
using System.Net;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzFuncIsoDocker
{
    public class Func1
    {
        private readonly ILogger _logger;

        private readonly TelemetryConfiguration _telemetryConfigurations;
        private readonly IEnumerable<ITelemetryInitializer> _telemetryInitializers;

        public Func1(
            ILoggerFactory loggerFactory,
            IEnumerable<ITelemetryInitializer> telemetryInitializers,
            IOptions<TelemetryConfiguration> telemetryConfigurations)
        {
            _logger = loggerFactory.CreateLogger<Func1>();
            _telemetryConfigurations = telemetryConfigurations?.Value ?? throw new System.ArgumentNullException(nameof(telemetryConfigurations));
            _telemetryInitializers = telemetryInitializers ?? throw new System.ArgumentNullException(nameof(telemetryInitializers));
        }

        [Function("Func1")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Here here: {confignull} - iKey: {ikey} - count: {count}", _telemetryConfigurations, _telemetryConfigurations?.InstrumentationKey, _telemetryConfigurations.TelemetryInitializers.Count);
            foreach (ITelemetryInitializer initializer in _telemetryConfigurations.TelemetryInitializers)
            {
                _logger.LogInformation("Telemetry initializer: {initializer}", initializer.GetType());
            }

            _logger.LogInformation("All telemetry initializers in the container");
            foreach(ITelemetryInitializer initializer1 in _telemetryInitializers)
            {
                _logger.LogInformation("Telemetry initializer: {initializer}", initializer1.GetType());
            }

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
