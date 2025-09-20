using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.Entidades;

namespace ConsorcioVeiculos.Dominio.Interfaces;
public interface IAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
    List<Administrador> Todos(int? pagina = 1 , string? email = null , string? senha = null);
    Administrador? BuscaPorId(int id);
    void Adicionar(Administrador administrador);
    void Atualizar(Administrador administrador);
    void Deletar(Administrador administrador);
}
