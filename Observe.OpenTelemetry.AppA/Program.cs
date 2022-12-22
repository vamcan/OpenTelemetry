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

    // Add services to the container.
    builder.Host.UseSerilog((context, config) =>
    {
        config.MinimumLevel.Information().WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "Sample-ApiA-{0:yyyy.MM}",
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
            }).Enrich.WithSpan(); 
    });
    builder.Services.AddHttpClient("ApiB", c => c.BaseAddress = new Uri("https://localhost:7104/"));
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
