using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.DTOs.Interfaces;
using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Dominio.ModelViews;
using ConsorcioVeiculos.Dominio.Servicos;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(op =>
    op.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")))
    );
builder.Services.AddScoped<IValidacaoDTO, ValidacaoDTO>();
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculosServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

#region Administradores
// post
app.MapPost("/administradores/login" , ([FromBody] LoginDTO loginDTO, IAdministradorServico admServico) =>
{
    if(admServico.Login(loginDTO) != null) return Results.Ok("Login realizado com sucesso!");
    else return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region Veiculos
// get todos
app.MapGet("/veiculos" , ([FromQuery]int? pagina, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.Todos(pagina);

    return Results.Ok(veiculos);
}).WithTags("Veículos");

// get id
app.MapGet("/veiculos/{id}" , ([FromRoute]int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);

    if(veiculo != null) return Results.Ok(veiculo);
    else return Results.NotFound();
}).WithTags("Veículos");

// post
app.MapPost("/veiculos" , ([FromBody] VeiculoDTO veiculoDTO , IVeiculosServico veiculosServico, IValidacaoDTO validacaoDTO) =>
{
    // validando dados obrigatórios
    var validacao = validacaoDTO.Validar(veiculoDTO);
    if(validacao.Mensagens.Any()) return Results.BadRequest(validacao);

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculosServico.Adicionar(veiculo);

    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
}).WithTags("Veículos");

// put
app.MapPut("/veiculos/{id}" , ([FromRoute]int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico, IValidacaoDTO validacaoDTO) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    // validando dados obrigatórios
    var validacao = validacaoDTO.Validar(veiculoDTO);
    if(validacao.Mensagens.Any()) return Results.BadRequest(validacao);

    // Atualiza os campos do veículo com os dados do DTO
    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;
    veiculosServico.Atualizar(veiculo);
    
    return Results.Ok(veiculo);
}).WithTags("Veículos");

// delete
app.MapDelete("/veiculos/{id}" , ([FromRoute]int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculosServico.Deletar(veiculo);

    return Results.Ok();
}).WithTags("Veículos");
#endregion

app.UseHttpsRedirection();

app.Run();
