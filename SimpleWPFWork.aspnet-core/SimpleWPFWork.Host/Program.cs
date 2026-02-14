using Serilog;
using SimpleWPFWork.Application;
using SimpleWPFWork.EntityFrameworkCore;
using SimpleWPFWork.Host.Middleware;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/app-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

Log.Information("Starting SimpleWPFWork Application...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddEntityFrameworkCore(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
    };
});
app.Use(async (context, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();

    await next();

    sw.Stop();

    var color = sw.ElapsedMilliseconds switch
    {
        < 10 => ConsoleColor.Green,
        < 50 => ConsoleColor.Yellow,
        _ => ConsoleColor.Red
    };

    Console.ForegroundColor = color;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] " +
        $"{context.Request.Method,-6} {context.Request.Path,-40} " +
        $"-> {context.Response.StatusCode} in {sw.ElapsedMilliseconds,4}ms");
    Console.ResetColor();
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();