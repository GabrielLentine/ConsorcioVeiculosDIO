namespace ConsorcioVeiculos.Dominio.DTOs;
public class LoginDTO
{
    public string Email { get; set; } = default;
    public string Senha { get; set; } = default;
    public string Perfil { get; set; }
}
