using ConsorcioVeiculos.Dominio.Entidades;

namespace Teste.Dominio.Entidades;

[TestClass]
public class AdministradorTeste
{
    [TestMethod]
    public void TestarGetSetPropriedades()
    {
        // Arrange
        var adm = new Administrador();

        // Act
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Senha = "senha123";
        adm.Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader;

        // Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("teste@teste.com", adm.Email);
        Assert.AreEqual("senha123", adm.Senha);
        Assert.AreEqual(ConsorcioVeiculos.Dominio.Enums.Perfil.Leader, adm.Perfil);
    }
}
