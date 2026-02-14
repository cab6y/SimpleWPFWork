using SimpleWPFWork.Application;
using SimpleWPFWork.EntityFrameworkCore;
using SimpleWPFWork.Host.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkCore(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
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