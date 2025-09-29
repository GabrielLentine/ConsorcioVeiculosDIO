using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;

namespace Teste.Mocks;

public class VeiculoServicoMock : IVeiculosServico
{
    private static List<Veiculo> _veiculos = new List<Veiculo>()
    {
        new Veiculo() { Id = 1, Nome = "Carro X", Marca = "Marca Y", Ano = 2020 },
        new Veiculo() { Id = 2, Nome = "Carro Y", Marca = "Marca Z", Ano = 2021 }
    };

    public List<Veiculo> Todos(int? pagina = 1 , string? nome = null , string? marca = null) => _veiculos;

    public void Adicionar(Veiculo veiculo)
    {
        veiculo.Id = _veiculos.Count() + 1;
        _veiculos.Add(veiculo);
    }

    public Veiculo? BuscaPorId(int id) => _veiculos.Find(v => v.Id == id);

    public void Atualizar(Veiculo veiculo)
    {
        throw new NotImplementedException();
    }

    public void Deletar(Veiculo veiculo)
    {
        throw new NotImplementedException();
    }
}
