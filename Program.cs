using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Dominio.Servicos;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DbContexto>(op =>
    op.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")))
    );
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// get
app.MapGet("/", () => "Hello World!");

// post
app.MapPost("/login" , ([FromBody] LoginDTO loginDTO, IAdministradorServico admServico) =>
{
    if(admServico.Login(loginDTO) != null) return Results.Ok("Login realizado com sucesso!");
    else return Results.Unauthorized();
});

app.UseHttpsRedirection();

app.Run();
