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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // ✅ Sadece bu!

app.Run();