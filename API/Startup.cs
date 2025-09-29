using ConsorcioVeiculos.Dominio.Authorization;
using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.DTOs.Interfaces;
using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Dominio.ModelViews;
using ConsorcioVeiculos.Dominio.Servicos;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConsorcioVeiculos;

public class Startup
{
    private string _key;
    public IConfiguration Configuration { get; set; } = default!;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _key = Configuration?.GetSection("Jwt")?.ToString() ?? "";
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen
            (op =>
            {
                // JWT
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


        services.AddDbContext<DbContexto>
        (op =>
            op.UseMySql(Configuration.GetConnectionString("MySql") ,
            ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql"))
        ));

        var key = Configuration.GetSection("Jwt").ToString();
        if(string.IsNullOrEmpty(key)) key = "123456";
        services.AddAuthentication
        (op =>
        {
            op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(op =>
        {
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true ,
                ValidateIssuerSigningKey = true ,
                ValidateIssuer = false ,
                ValidateAudience = false ,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) ,
                RoleClaimType = ClaimTypes.Role
            };
        });
        services.AddAuthorization();

        // Serviços
        services.AddScoped<IValidacaoDTO<VeiculoDTO> , ValidarVeiculoDTO>();
        services.AddScoped<IValidacaoDTO<AdministradorDTO> , ValidarAdministradorDTO>();
        services.AddScoped<IAdministradorServico , AdministradorServico>();
        services.AddScoped<IVeiculosServico , VeiculosServico>();
    }

    public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(end =>
        {
            end.MapGet("/" , () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");

            #region Administradores
            string GerarTokenJWT(Administrador administrador)
            {
                if(string.IsNullOrEmpty(_key)) return string.Empty;

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
                var credentials = new SigningCredentials(securityKey , SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, administrador.Email),
                    new Claim(ClaimTypes.Role, administrador.Perfil.ToString())
                };
                var token = new JwtSecurityToken
                (
                    claims: claims ,
                    expires: DateTime.Now.AddDays(1) ,
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            // get todos
            end.MapGet("/administradores" , (IAdministradorServico admServico) =>
            {
                var admins = admServico.Todos(1);
                return Results.Ok(admins);
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Administradores")
              .WithDescription("Mostra todos os Leaders/Admins cadastrados no sistema.");

            // get id
            end.MapGet("/administradores/{id}" , ([FromRoute] int id , IAdministradorServico admServico) =>
            {
                var admin = admServico.BuscaPorId(id);

                if(admin != null) return Results.Ok(admin);
                else return Results.NotFound("Administrador não encontrado. Por favor, verifique o ID digitado e tente novamente!");
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Administradores")
              .WithDescription("Mostra um Leader/Admin pelo seu ID.");

            // post
            end.MapPost("/administradores/cadastro" , ([FromBody] AdministradorDTO adminDTO , IAdministradorServico admServico , IValidacaoDTO<AdministradorDTO> validacaoDTO) =>
            {
                // validando dados obrigatórios
                var validacao = validacaoDTO.Validar(adminDTO);
                if(validacao.Mensagens.Any()) return Results.BadRequest(validacao);

                var newAdmin = new Administrador
                {
                    Email = adminDTO.Email ,
                    Senha = adminDTO.Senha ,
                    Perfil = adminDTO.Perfil
                };
                admServico.Adicionar(newAdmin);

                return Results.Created($"/administradores/{newAdmin.Id}" , new { newAdmin.Id , newAdmin.Email , newAdmin.Perfil });
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Administradores")
              .WithDescription("Cadastra um novo Leader/Admin. || Perfil = 1 -> Leader | Perfil = 2 -> Admin");

            end.MapPost("/administradores/login" , ([FromBody] LoginDTO loginDTO , IAdministradorServico admServico) =>
            {
                var admin = admServico.Login(loginDTO);

                if(admin != null)
                {
                    string token = GerarTokenJWT(admin);
                    return Results.Ok(new AdmLogado
                    {
                        Email = admin.Email ,
                        Perfil = admin.Perfil ,
                        Token = token
                    });
                }
                else
                {
                    return Results.Unauthorized();
                }
            }).AllowAnonymous()
              .WithTags("Administradores")
              .WithDescription("Loga com um Leader/Admin existente.");

            // put
            end.MapPut("/administradores/{id}" , ([FromRoute] int id , [FromBody] AdministradorDTO adminDTO , IAdministradorServico admServico , IValidacaoDTO<AdministradorDTO> validacaoDTO) =>
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
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Administradores")
              .WithDescription("Altera dados de um Leader/Admin pelo seu ID.");

            // delete
            end.MapDelete("/administradores/{id}" , ([FromRoute] int id , IAdministradorServico admServico) =>
            {
                var admin = admServico.BuscaPorId(id);
                if(admin == null) return Results.NotFound();

                admServico.Deletar(admin);

                return Results.Ok();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Administradores")
              .WithDescription("Deleta um Leader/Admin pelo seu ID.");
            #endregion

            #region Veiculos
            // get todos
            end.MapGet("/veiculos" , ([FromQuery] int? pagina , IVeiculosServico veiculosServico) =>
            {
                var veiculos = veiculosServico.Todos(pagina);

                return Results.Ok(veiculos);
            })
                .RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader},{Roles.Admin}" })
                .WithTags("Veículos")
                .WithDescription("Mostra todos os Veículos cadastrados no sistema.");

            // get id
            end.MapGet("/veiculos/{id}" , ([FromRoute] int id , IVeiculosServico veiculosServico) =>
            {
                var veiculo = veiculosServico.BuscaPorId(id);

                if(veiculo != null) return Results.Ok(veiculo);
                else return Results.NotFound("Veículo não encontrado. Por favor, verifique o ID digitado e tente novamente!");
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}, {Roles.Admin}" })
              .WithTags("Veículos")
              .WithDescription("Mostra um Veículo pelo seu ID.");

            // post
            end.MapPost("/veiculos" , ([FromBody] VeiculoDTO veiculoDTO , IVeiculosServico veiculosServico , IValidacaoDTO<VeiculoDTO> validacaoDTO) =>
            {
                // validando dados obrigatórios
                var validacao = validacaoDTO.Validar(veiculoDTO);
                if(validacao.Mensagens.Any()) return Results.BadRequest(validacao);

                var veiculo = new Veiculo
                {
                    Nome = veiculoDTO.Nome ,
                    Marca = veiculoDTO.Marca ,
                    Ano = veiculoDTO.Ano
                };
                veiculosServico.Adicionar(veiculo);

                return Results.Created($"/veiculos/{veiculo.Id}" , veiculo);
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}, {Roles.Admin}" })
              .WithTags("Veículos")
              .WithDescription("Cadastra um novo Veículo.");

            // put
            end.MapPut("/veiculos/{id}" , ([FromRoute] int id , [FromBody] VeiculoDTO veiculoDTO , IVeiculosServico veiculosServico , IValidacaoDTO<VeiculoDTO> validacaoDTO) =>
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
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Veículos")
              .WithDescription("Altera dados de um Veículo pelo seu ID.");

            // delete
            end.MapDelete("/veiculos/{id}" , ([FromRoute] int id , IVeiculosServico veiculosServico) =>
            {
                var veiculo = veiculosServico.BuscaPorId(id);
                if(veiculo == null) return Results.NotFound();

                veiculosServico.Deletar(veiculo);

                return Results.Ok();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = $"{Roles.Leader}" })
              .WithTags("Veículos")
              .WithDescription("Deleta um Veículo pelo seu ID.");
            #endregion
        });
    }
}
