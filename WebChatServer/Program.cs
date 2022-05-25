using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebChatServer.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebChatServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebChatServerContext") ?? throw new InvalidOperationException("Connection string 'WebChatServerContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

