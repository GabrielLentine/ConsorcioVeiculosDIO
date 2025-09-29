using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Interfaces;

namespace Teste.Mocks;

public class AdministradorServicoMock : IAdministradorServico
{
    private static List<Administrador> _administradores = new List<Administrador>()
    {
        new Administrador() { Id = 1, Email = "leader@teste.com", Senha = "123456", Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Leader },
        new Administrador() { Id = 1, Email = "adm@teste.com", Senha = "senha123", Perfil = ConsorcioVeiculos.Dominio.Enums.Perfil.Admin }
    };

    public Administrador? Login(LoginDTO loginDTO) => _administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha && a.Perfil == loginDTO.Perfil);

    public List<Administrador> Todos(int? pagina = 1 , string? email = null , string? senha = null) => _administradores;

    public void Adicionar(Administrador administrador)
    {
        administrador.Id = _administradores.Count() + 1;
        _administradores.Add(administrador);
    }

    public Administrador? BuscaPorId(int id) => _administradores.Find(a => a.Id == id);

    public void Atualizar(Administrador administrador)
    {
        throw new NotImplementedException();
    }

    public void Deletar(Administrador administrador)
    {
        throw new NotImplementedException();
    }
}
