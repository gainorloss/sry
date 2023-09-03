namespace Microsoft.AspNetCore.Routing
{
    public static class EndpointBuilderExtensions
    {
        public static IEndpointConventionBuilder MapEnvironments(this IEndpointRouteBuilder app)
        {
            return app.MapGet("/api/env", async ctx =>
               {
                   var variables = Environment.GetEnvironmentVariables();
                   var map = new Dictionary<string, object>();
                   map.Add("code", 200);
                   map.Add("data", variables);
                   variables.Add("processor_count", Environment.ProcessorCount);
                   await ctx.Response.WriteAsJsonAsync(map);
               });
        }
    }
}
