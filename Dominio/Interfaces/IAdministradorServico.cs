using ConsorcioVeiculos.Dominio.DTOs;
using ConsorcioVeiculos.Dominio.Entidades;

namespace ConsorcioVeiculos.Dominio.Interfaces;
public interface IAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
}
