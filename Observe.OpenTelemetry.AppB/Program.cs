using Observe.OpenTelemetry.AppB.Model;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Elasticsearch;

Log.Logger = new LoggerConfiguration().WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();
try
{
    Log.Information("Application Start");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, config) =>
    {
        config.MinimumLevel.Information().WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "Sample-ApiB-{0:yyyy.MM}",
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
            }).Enrich.WithSpan(); ;
    });
    var serviceName = "Samples.ApiB ";
    var serviceVersion = "1.0.0";
    builder.Services.AddOpenTelemetryTracing(traceProviderBuilder =>
    {
        traceProviderBuilder
            .AddConsoleExporter()
            .AddJaegerExporter()
            .AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
            )
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddSqlClientInstrumentation();
    });
    // Add services to the container.
    builder.Services.AddDbContext<PersonDbContext>();
    builder.Services.AddScoped<PersonRepo>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{

    Log.Error(ex, "Application Error");
}
finally
{
    Log.CloseAndFlush();
}
