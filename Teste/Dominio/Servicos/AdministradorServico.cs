using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;

[TestClass]
public class AdministradorServico
{
    [TestMethod]
    public void TestarSalvarAdministrador()
    {
        // Arrange
        var contexto = CriarContexto();
        var admServico = new ConsorcioVeiculos.Dominio.Servicos.AdministradorServico(contexto);

        var adm = new Administrador
        {
            Email = "teste@teste.com" ,
            Senha = "senha123" ,
            Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader
        };

        // Act
        admServico.Adicionar(adm);

        // Assert
        Assert.AreEqual(1 , admServico.Todos(1).Count());
    }

    [TestMethod]
    public void TestarBuscarPorId()
    {
        // Arrange
        var contexto = CriarContexto();
        var admServico = new ConsorcioVeiculos.Dominio.Servicos.AdministradorServico(contexto);

        var adm = new Administrador
        {
            Email = "teste@teste.com" ,
            Senha = "senha123" ,
            Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader
        };
        admServico.Adicionar(adm);

        // Act
        var admDoBanco = admServico.BuscaPorId(adm.Id);

        // Assert
        Assert.AreEqual(adm.Id , admDoBanco.Id);
    }

    private DbContexto CriarContexto()
    {
        // Cada teste recebe um banco InMemory único
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DbContexto(options);
    }
}
