using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.ModelViews;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Teste.Helpers;

namespace Teste.Requests;

[TestClass]
public class VeiculoRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext) => Setup.ClassInit(testContext);

    [ClassCleanup]
    public static void ClassCleanup() => Setup.ClassCleanup();

    [TestMethod]
    public async Task TestarGetSetPropriedades()
    {
        // Arrange
        var veiculoDTO = new VeiculoDTO
        {
            Nome = "Carro Z" ,
            Marca = "Marca W" ,
            Ano = 2022
        };
        var conteudoCarro = new StringContent(JsonSerializer.Serialize(veiculoDTO) , Encoding.UTF8 , "application/json");

        // login para obter token
        var loginDTO = new LoginDTO
        {
            Email = "leader@teste.com" ,
            Senha = "123456" ,
            Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader
        };
        var conteudoLogin = new StringContent(JsonSerializer.Serialize(loginDTO) , Encoding.UTF8 , "application/json");
        var respostaLogin = await Setup.client.PostAsync("/administradores/login" , conteudoLogin);
        respostaLogin.EnsureSuccessStatusCode();

        var resultadoLogin = await respostaLogin.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(resultadoLogin);

        var token = doc.RootElement.GetProperty("token").GetString();
        if(string.IsNullOrEmpty(token)) throw new Exception("Token não pode ser nulo ou vazio");

        // Act
        var requisicao = new HttpRequestMessage(HttpMethod.Post , "/veiculos")
        {
            Content = conteudoCarro
        };
        requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer" , token);

        var resposta = await Setup.client.SendAsync(requisicao);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created , resposta.StatusCode);

        var resultado = await resposta.Content.ReadAsStringAsync();
        var veiculoSetado = JsonSerializer.Deserialize<VeiculoSetado>(resultado , new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(veiculoSetado?.Nome ?? "");
        Assert.IsNotNull(veiculoSetado?.Marca ?? "");
        Assert.IsNotNull(veiculoSetado?.Ano);
    }
}
