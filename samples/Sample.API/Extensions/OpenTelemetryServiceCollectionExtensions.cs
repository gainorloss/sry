using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenTelemetryServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration, string serviceName)
        {

            var tracingOtlpEndpoint = configuration["OpenTelemetry:OTLP_ENDPOINT_URL"];
            var zipkinEndpoint = configuration["OpenTelemetry:ZIPKIN_ENDPOINT_URL"];
            var otel = services.AddOpenTelemetry();

            // Configure OpenTelemetry Resources with the application name
            otel.ConfigureResource(resource => resource
                .AddService(serviceName));

            // Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
            otel.WithMetrics(metrics => metrics
                // Metrics provider from OpenTelemetry
                .AddAspNetCoreInstrumentation()
                //.AddMeter(greeterMeter.Name)
                // Metrics provides by ASP.NET Core in .NET 8
                .AddMeter("Microsoft.AspNetCore.Hosting")
                .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                .AddPrometheusExporter());

            // Add Tracing for ASP.NET Core and our custom ActivitySource and export to Jaeger
            otel.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                //tracing.AddSource(greeterActivitySource.Name);
                tracing.AddConsoleExporter();
                if (tracingOtlpEndpoint != null)
                {
                    tracing.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
                    });
                }
                if (zipkinEndpoint != null)
                {
                    tracing.AddZipkinExporter(opt =>
                    {
                        opt.Endpoint = new Uri($"{zipkinEndpoint}/api/v2/spans");
                    });
                }
            });
            return services;
        }
    }
}
