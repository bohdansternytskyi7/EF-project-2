using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TripCompany.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TripCompanyContext>(opt =>
{
    opt.UseSqlServer("Server=db-mssql16.pjwstk.edu.pl;Database=s26028;Integrated Security=True;TrustServerCertificate=True");
    opt.LogTo(Console.WriteLine);
});

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();
