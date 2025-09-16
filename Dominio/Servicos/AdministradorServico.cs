using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;
using ConsorcioVeiculos.Infraestrutura.DB;

namespace ConsorcioVeiculos.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _db;

    public AdministradorServico(DbContexto db)
    {
        _db = db;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        return _db.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
    }
}
