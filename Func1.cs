using System.Collections.Generic;
using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncIsoDocker
{
    public class Func1
    {
        private readonly ILogger _logger;

        private readonly IEnumerable<ITelemetryInitializer> _telemetryInitializers;
        private readonly TelemetryClient _telemetryClient;

        public Func1(
            ILoggerFactory loggerFactory,
            TelemetryClient telemetryClient)
        {
            _logger = loggerFactory.CreateLogger<Func1>();
            _telemetryClient = telemetryClient ?? throw new System.ArgumentNullException(nameof(telemetryClient));
        }

        [Function("Func1")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            using (IOperationHolder<RequestTelemetry> operation = _telemetryClient.StartOperation<RequestTelemetry>("Func1Request"))
            {
                try
                {
                    // Warning will be sent to application insights.
                    _logger.LogWarning("This is a warning!");

                    _logger.LogInformation("C# HTTP trigger function processed a request.");    // By default, this won't be sent to AI.

                    var response = req.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString("Welcome to Azure Functions!");

                    return response;
                }
                catch
                {
                    operation.Telemetry.Success = false;
                    throw;
                }
            }
        }
    }
}
