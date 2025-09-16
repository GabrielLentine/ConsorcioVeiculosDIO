using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Infraestrutura.DB;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// get
app.MapGet("/", () => "Hello World!");

// post
app.MapPost("/login" , (LoginDTO loginDTO) =>
{
    if(loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456") return Results.Ok("Login realizado com sucesso!");
    else return Results.Unauthorized();
});

app.UseHttpsRedirection();

app.Run();
