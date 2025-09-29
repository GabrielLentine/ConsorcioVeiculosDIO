using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Teste.Dominio.Servicos;

[TestClass]
public class AdministradorServico
{
    [TestMethod]
    public void TestarSalvarAdministrador()
    {
        // Arrange
        var contexto = CriarContexto();
        contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm = new Administrador();
        adm.Email = "teste@teste.com";
        adm.Senha = "senha123";
        adm.Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader;
        var admServico = new ConsorcioVeiculos.Dominio.Servicos.AdministradorServico(contexto);

        // Act
        admServico.Adicionar(adm);

        // Assert
        Assert.AreEqual(1 , admServico.Todos(1).Count());
    }

    public void TestarBuscarPorId()
    {
        // Arrange
        var contexto = CriarContexto();
        contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm = new Administrador();
        adm.Email = "teste@teste.com";
        adm.Senha = "senha123";
        adm.Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader;
        var admServico = new ConsorcioVeiculos.Dominio.Servicos.AdministradorServico(contexto);

        // Act
        var admDoBanco = admServico.BuscaPorId(adm.Id);

        // Assert
        Assert.AreEqual(1 , admDoBanco.Id);
    }
    
    private DbContexto CriarContexto()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        var configuration = builder.Build();

        return new DbContexto(configuration);
    }
}
