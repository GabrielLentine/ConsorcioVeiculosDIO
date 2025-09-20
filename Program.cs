using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.DTOs.Interfaces;
using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Dominio.ModelViews;
using ConsorcioVeiculos.Dominio.Servicos;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Serviços e Injeção de Dependência
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen
    (op =>
    {
        op.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme
        {
            Name = "Authorization" ,
            Type = SecuritySchemeType.Http ,
            Scheme = "bearer" ,
            BearerFormat = "JWT" ,
            In = ParameterLocation.Header ,
            Description = "Insira o token JWT aqui: {seu token}"
        });

        op.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });


builder.Services.AddDbContext<DbContexto>
(op =>
    op.UseMySql(builder.Configuration.GetConnectionString("MySql"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
));

var key = builder.Configuration.GetSection("Jwt").ToString();
if(string.IsNullOrEmpty(key)) key = "123456";
builder.Services.AddAuthentication
(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IValidacaoDTO<VeiculoDTO>, ValidarVeiculoDTO>();
builder.Services.AddScoped<IValidacaoDTO<AdministradorDTO>, ValidarAdministradorDTO>();
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculosServico>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");

#region Administradores
string GerarTokenJWT(Administrador administrador)
{
    if(string.IsNullOrEmpty(key)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
    };
    var token = new JwtSecurityToken
    (
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

// get todos
app.MapGet("/administradores" , (IAdministradorServico admServico) =>
{
    var admins = admServico.Todos();

    return Results.Ok(admins);
}).AllowAnonymous().RequireAuthorization().WithTags("Administradores");

// get id
app.MapGet("/administradores/{id}" , ([FromRoute]int id, IAdministradorServico admServico) =>
{
    var admin = admServico.BuscaPorId(id);

    if(admin != null) return Results.Ok(admin);
    else return Results.NotFound("Administrador não encontrado. Por favor, verifique o ID digitado e tente novamente!");
}).RequireAuthorization().WithTags("Administradores");

// post
app.MapPost("/administradores/cadastro", ([FromBody] AdministradorDTO adminDTO, IAdministradorServico admServico, IValidacaoDTO<AdministradorDTO> validacaoDTO) =>
{
    // validando dados obrigatórios
    var validacao = validacaoDTO.Validar(adminDTO);
    if(validacao.Mensagens.Any()) return Results.BadRequest(validacao);

    var newAdmin = new Administrador
    {
        Email = adminDTO.Email,
        Senha = adminDTO.Senha,
        Perfil = adminDTO.Perfil
    };
    admServico.Adicionar(newAdmin);

    return Results.Created($"/administradores/{newAdmin.Id}", new { newAdmin.Id, newAdmin.Email, newAdmin.Perfil });
}).RequireAuthorization().WithTags("Administradores");

app.MapPost("/administradores/login" , ([FromBody] LoginDTO loginDTO, IAdministradorServico admServico) =>
{
    var admin = admServico.Login(loginDTO);

    if(admin != null)
    {
        string token = GerarTokenJWT(admin);
        return Results.Ok(new AdmLogado
        {
            Email = admin.Email,
            Perfil = admin.Perfil,
            Token = token
        });
    }
    else
    {
        return Results.Unauthorized();
    }
}).AllowAnonymous().WithTags("Administradores");

// put
app.MapPut("/administradores/{id}" , ([FromRoute]int id, [FromBody] AdministradorDTO adminDTO, IAdministradorServico admServico, IValidacaoDTO<AdministradorDTO> validacaoDTO) =>
{
    var administrador = admServico.BuscaPorId(id);
    if(administrador == null) return Results.NotFound();

    // validando dados obrigatórios
    var validacao = validacaoDTO.Validar(adminDTO);
    if(validacao.Mensagens.Any()) return Results.BadRequest(validacao);

    // Atualiza os campos do administrador com os dados do DTO
    administrador.Email = adminDTO.Email;
    administrador.Senha = adminDTO.Senha;
    administrador.Perfil = adminDTO.Perfil;
    admServico.Atualizar(administrador);
    
    return Results.Ok(administrador);
}).WithTags("Administradores");

// delete
app.MapDelete("/administradores/{id}" , ([FromRoute]int id, IAdministradorServico admServico) =>
{
    var admin = admServico.BuscaPorId(id);
    if(admin == null) return Results.NotFound();

    admServico.Deletar(admin);

    return Results.Ok();
}).WithTags("Administradores");
#endregion

#region Veiculos
// get todos
app.MapGet("/veiculos" , ([FromQuery]int? pagina, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.Todos(pagina);

    return Results.Ok(veiculos);
}).RequireAuthorization().WithTags("Veículos");

// get id
app.MapGet("/veiculos/{id}" , ([FromRoute]int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);

    if(veiculo != null) return Results.Ok(veiculo);
    else return Results.NotFound("Veículo não encontrado. Por favor, verifique o ID digitado e tente novamente!");
}).RequireAuthorization().WithTags("Veículos");

// post
app.MapPost("/veiculos" , ([FromBody] VeiculoDTO veiculoDTO , IVeiculosServico veiculosServico, IValidacaoDTO<VeiculoDTO> validacaoDTO) =>
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
}).RequireAuthorization().WithTags("Veículos");

// put
app.MapPut("/veiculos/{id}" , ([FromRoute]int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico, IValidacaoDTO<VeiculoDTO> validacaoDTO) =>
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
}).RequireAuthorization().WithTags("Veículos");

// delete
app.MapDelete("/veiculos/{id}" , ([FromRoute]int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculosServico.Deletar(veiculo);

    return Results.Ok();
}).RequireAuthorization().WithTags("Veículos");
#endregion

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthentication();

app.Run();
