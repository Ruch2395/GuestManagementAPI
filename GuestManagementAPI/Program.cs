using GuestManagementAPI.Middleware;
using GuestManagementAPI.Data;
using GuestManagementAPI.Data.Repositories;
using GuestManagementAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Guest Management API", Version = "v1" });
});

// Register DbContext (in-memory provider omitted so project builds without extra package)
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseInMemoryDatabase("GuestManagementDb")
    );

builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API v1"));
}

// Provide a root endpoint to avoid 404 at '/'
app.MapGet("/", () => Results.Redirect("/swagger", permanent: false));

//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
