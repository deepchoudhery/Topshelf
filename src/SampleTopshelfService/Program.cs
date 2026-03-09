using Serilog;
using SampleTopshelfService;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Detect whether running as a Windows service or interactively.
    // When installed as a service, UseWindowsService() handles lifetime events automatically.
    // To install: sc.exe create SampleTopshelfService binPath="<path>\SampleTopshelfService.exe"
    // To uninstall: sc.exe delete SampleTopshelfService
    builder.Host.UseWindowsService(options =>
    {
        options.ServiceName = "SampleTopshelfService";
    });

    builder.Host.UseSerilog();

    // Options can be set via appsettings.json ("SampleService" section) or
    // command line (e.g. --SampleService:ThrowOnStart=true)
    builder.Services.Configure<SampleServiceOptions>(
        builder.Configuration.GetSection("SampleService"));

    builder.Services.AddHostedService<SampleWorker>();

    var app = builder.Build();

    app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

    await app.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
