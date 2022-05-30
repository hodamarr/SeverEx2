using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebChatServer.Data;
using WebChatServer.Hubs;


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
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("http://localhost:3000")
    .AllowCredentials(); ;
}));


builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthorization();
//app.UseSession();
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.Run();

