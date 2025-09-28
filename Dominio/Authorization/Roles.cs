using ConsorcioVeiculos.Dominio.Enums;

namespace ConsorcioVeiculos.Dominio.Authorization
{
    public static class Roles
    {
        public const string Leader = nameof(Perfil.Leader);
        public const string Admin = nameof(Perfil.Admin);
    }
}
