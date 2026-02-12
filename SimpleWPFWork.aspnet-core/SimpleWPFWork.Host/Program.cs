using SimpleWPFWork.Application;
using SimpleWPFWork.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ? EntityFrameworkCore katmaný
builder.Services.AddEntityFrameworkCore(builder.Configuration);

// ? Application katmaný (AutoMapper + tüm AppService'ler)
builder.Services.AddApplicationLayer();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();