using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Teste.Dominio.Servicos;

[TestClass]
public class VeiculoServico
{
    [TestMethod]
    public void TestarSalvarVeiculo()
    {
        // Arrange
        var contexto = CriarContexto();
        contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo();
        veiculo.Nome = "Modelo X";
        veiculo.Marca = "Marca Y";
        veiculo.Ano = 2023;
        var veiculosServico = new ConsorcioVeiculos.Dominio.Servicos.VeiculosServico(contexto);

        // Act
        veiculosServico.Adicionar(veiculo);

        // Assert
        Assert.AreEqual(1 , veiculosServico.Todos().Count());
    }

    public void TestarBuscarPorId()
    {
        // Arrange
        var contexto = CriarContexto();
        contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo();
        veiculo.Nome = "Modelo X";
        veiculo.Marca = "Marca Y";
        veiculo.Ano = 2023;
        var veiculosServico = new ConsorcioVeiculos.Dominio.Servicos.VeiculosServico(contexto);

        // Act
        var veiculoDoBanco = veiculosServico.BuscaPorId(veiculo.Id);

        // Assert
        Assert.AreEqual(1 , veiculoDoBanco.Id);
    }

    private DbContexto CriarContexto()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "" , ".." , ".." , ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json" , optional: false , reloadOnChange: true)
            .AddEnvironmentVariables();
        var configuration = builder.Build();

        return new DbContexto(configuration);
    }
}
