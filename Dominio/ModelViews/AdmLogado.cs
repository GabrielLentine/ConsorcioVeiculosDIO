using ConsorcioVeiculos.Dominio.Enums;

namespace ConsorcioVeiculos.Dominio.ModelViews;
public class AdmLogado
{
    public string? Email { get; set; } = default!;
    public Perfil Perfil { get; set; } = default!;
    public string? Token { get; set; } = default!;
}
