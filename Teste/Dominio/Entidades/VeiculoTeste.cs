using ConsorcioVeiculos.Dominio.Entidades;

namespace Teste.Dominio.Entidades;

[TestClass]
public class VeiculoTeste
{
    [TestMethod]
    public void TestarGetSetPropriedades()
    {
        // Arrange
        var veiculo = new Veiculo();

        // Act
        veiculo.Id = 1;
        veiculo.Nome = "Modelo X";
        veiculo.Marca = "Marca Y";
        veiculo.Ano = 2023;

        // Assert
        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("Modelo X", veiculo.Nome);
        Assert.AreEqual("Marca Y", veiculo.Marca);
        Assert.AreEqual(2023, veiculo.Ano);
    }
}
