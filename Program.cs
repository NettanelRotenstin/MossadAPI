using MossadAPI.DAL;
using Microsoft.EntityFrameworkCore;
using MossadAPI.MiddleWares;
using MossadAPI.MiddleWares.Global;
using MossadAPI.Controllers;


var builder = WebApplication.CreateBuilder(args);
//adding the connect to db
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContextMossadAPI>(options => options.UseSqlServer(connectionString));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DBContextMossadAPI>();
 



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseMiddleware<auth>();

//app.UseWhen(
//    context =>
//        context.Request.Path.StartsWithSegments("/api/targets"),
//    appBuilder =>
//    {
//        appBuilder.UseMiddleware<auth>();
//        appBuilder.UseMiddleware<JwtAuth>();
//    });
app.MapControllers();

app.Run();
