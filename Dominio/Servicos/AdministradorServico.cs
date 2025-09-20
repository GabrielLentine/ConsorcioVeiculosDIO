using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;

namespace ConsorcioVeiculos.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _contexto;

    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        return _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
    }

    public List<Administrador> Todos(int? pagina = 1 , string? email = null , string? senha = null)
    {
        var query = _contexto.Administradores.AsQueryable();

        // paginação
        int itensPorPagina = 10;
        if(pagina != null) query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        // email
        if(!string.IsNullOrEmpty(email)) query = query.Where(a => EF.Functions.Like(a.Email.ToLower(), $"%{email}%"));

        // senha
        if(!string.IsNullOrEmpty(senha)) query = query.Where(a => EF.Functions.Like(a.Senha.ToLower(), $"%{senha}%"));

        return query.ToList();
    }

    public Administrador? BuscaPorId(int id)
    {
        return _contexto.Administradores.Find(id) ?? throw new KeyNotFoundException("Administrador não encontrado.");
    }

    public void Adicionar(Administrador administrador)
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();
    }

    public void Atualizar(Administrador administrador)
    {
        _contexto.Administradores.Update(administrador);
        _contexto.SaveChanges();
    }

    public void Deletar(Administrador administrador)
    {
        _contexto.Administradores.Remove(administrador);
        _contexto.SaveChanges();
    }
}
