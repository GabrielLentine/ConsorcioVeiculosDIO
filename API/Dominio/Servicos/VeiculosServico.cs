using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;

namespace ConsorcioVeiculos.Dominio.Servicos
{
    public class VeiculosServico : IVeiculosServico
    {
        private readonly DbContexto _contexto;

        public VeiculosServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public List<Veiculo> Todos(int? pagina = 1 , string? nome = null , string? marca = null)
        {
            var query = _contexto.Veiculos.AsQueryable();

            // paginação
            int itensPorPagina = 10;
            if(pagina != null) query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

            // veiculo
            if(!string.IsNullOrEmpty(nome)) query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome}%"));

            // marca
            if(!string.IsNullOrEmpty(marca)) query = query.Where(v => EF.Functions.Like(v.Marca.ToLower(), $"%{marca}%"));

            return query.ToList();
        }

        public Veiculo? BuscaPorId(int id)
        {
            if(id <= 0) return null;

            return _contexto.Veiculos.Find(id);
        }

        public void Adicionar(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public void Deletar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }
    }
}
