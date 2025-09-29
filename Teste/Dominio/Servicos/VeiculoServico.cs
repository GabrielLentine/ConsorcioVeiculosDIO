using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;

[TestClass]
public class VeiculoServico
{
    [TestMethod]
    public void TestarSalvarVeiculo()
    {
        // Arrange
        var contexto = CriarContexto();
        var veiculosServico = new ConsorcioVeiculos.Dominio.Servicos.VeiculosServico(contexto);

        var veiculo = new Veiculo
        {
            Nome = "Modelo X" ,
            Marca = "Marca Y" ,
            Ano = 2023
        };

        // Act
        veiculosServico.Adicionar(veiculo);

        // Assert
        Assert.AreEqual(1 , veiculosServico.Todos().Count());
    }

    [TestMethod]
    public void TestarBuscarPorId()
    {
        // Arrange
        var contexto = CriarContexto();
        var veiculosServico = new ConsorcioVeiculos.Dominio.Servicos.VeiculosServico(contexto);

        var veiculo = new Veiculo
        {
            Nome = "Modelo X" ,
            Marca = "Marca Y" ,
            Ano = 2023
        };
        veiculosServico.Adicionar(veiculo);

        // Act
        var veiculoDoBanco = veiculosServico.BuscaPorId(veiculo.Id);

        // Assert
        Assert.AreEqual(veiculo.Id , veiculoDoBanco.Id);
    }

    private DbContexto CriarContexto()
    {
        // Cada teste recebe um banco InMemory único
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DbContexto(options);
    }
}
