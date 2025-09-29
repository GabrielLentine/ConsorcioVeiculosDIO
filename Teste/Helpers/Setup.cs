using ConsorcioVeiculos;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Dominio.Servicos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Teste.Mocks;

namespace Teste.Helpers;

public static class Setup
{
    public const string PORT = "5001";
    public static TestContext context = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public static HttpClient client = default!;

    public static void ClassInit(TestContext testContext)
    {
        context = testContext;
        http = new WebApplicationFactory<Startup>();

        http = http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("https_port", PORT).UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IAdministradorServico , AdministradorServicoMock>();
                services.AddScoped<IVeiculosServico , VeiculoServicoMock>();
            });
        });

        client = http.CreateClient();
    }

    public static void ClassCleanup()
    {
    }
}

