using ConsorcioVeiculos.Dominio.Entidades;

namespace ConsorcioVeiculos.Dominio.Interfaces
{
    public interface IVeiculosServico
    {
        List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null);
        Veiculo? BuscaPorId(int id);
        void Adicionar(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Deletar(Veiculo veiculo);
    }
}
