using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.ModelViews;
using System.Net;
using System.Text;
using System.Text.Json;
using Teste.Helpers;

namespace Teste.Requests;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext) => Setup.ClassInit(testContext);

    [ClassCleanup]
    public static void ClassCleanup() => Setup.ClassCleanup();

    [TestMethod]
    public async Task TestarGetSetPropriedades()
    {
        // Arrange
        var loginDTO = new LoginDTO
        {
            Email = "leader@teste.com" ,
            Senha = "123456" ,
            Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader
        };

        var conteudo = new StringContent(JsonSerializer.Serialize(loginDTO) , Encoding.UTF8 , "application/json");

        // Act
        var resposta = await Setup.client.PostAsync("/administradores/login" , conteudo);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK , resposta.StatusCode);

        var resultado = await resposta.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdmLogado>(resultado , new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Perfil);
        Assert.IsNotNull(admLogado?.Token ?? "");
    }
}
