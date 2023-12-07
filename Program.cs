using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Data;
using WebApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<AlquilerDb>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear la solicitud POST para agregar un vehículo a la base de datos
app.MapPost("/vehiculos/", async (Vehiculo v, AlquilerDb db) =>
{
    db.Vehiculos.Add(v);
    await db.SaveChangesAsync();

    return Results.Created($"/vehiculo/{v.Id}", v);
});

// Mapear la solicitud GET para obtener un vehículo por su Id
app.MapGet("/vehiculos/{Id:int}", async (int Id, AlquilerDb db) =>
{
    return await db.Vehiculos.FindAsync(Id)
        is Vehiculo e
        ? Results.Ok(e)
        : Results.NotFound();
});

// Mapear la solicitud GET para obtener todos los vehículos
app.MapGet("/vehiculos", async (AlquilerDb db) => await db.Vehiculos.ToListAsync());

// Mapear la solicitud PUT para actualizar la información de un vehículo por su Id
app.MapPut("/vehiculos/{Id:int}", async (int Id, Vehiculo v, AlquilerDb db) =>
{
    if (v.Id != Id)
        return Results.BadRequest();
    var vehiculo = await db.Vehiculos.FindAsync(Id);
    
    if (vehiculo is null) return Results.NotFound();

    vehiculo.Marca = v.Marca;
    vehiculo.Modelo = v.Modelo;
    vehiculo.Año = v.Año;
    vehiculo.Color = v.Color;
    vehiculo.Placa = v.Placa;
    vehiculo.PrecioAlquilerPorDia = v.PrecioAlquilerPorDia;
    vehiculo.Disponibilidad = v.Disponibilidad;

    await db.SaveChangesAsync();

    return Results.Ok(vehiculo);
});

// Mapear la solicitud DELETE para eliminar un vehículo por su Id
app.MapDelete("/vehiculos/{Id:int}", async (int Id, AlquilerDb db) =>
{
    var vehiculo = await db.Vehiculos.FindAsync(Id);

    if (vehiculo is null) return Results.NotFound();

    db.Vehiculos.Remove(vehiculo);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();