using ConsorcioVeiculos.Dominio.DTOs.Interfaces;
using ConsorcioVeiculos.Dominio.ModelViews;

namespace ConsorcioVeiculos.Dominio.DTOs
{
    public class ValidarAdministradorDTO : IValidacaoDTO<AdministradorDTO>
    {
        public ErrosDeValidacao Validar(AdministradorDTO administradorDTO)
        {
            var validacao = new ErrosDeValidacao();

            // email
            if(!administradorDTO.Email.Contains("@") || !administradorDTO.Email.Contains(".com")) validacao.Mensagens.Add("O email deve ser válido.");

            // senha
            if(administradorDTO.Senha.Length < 6) validacao.Mensagens.Add("A senha deve ter no mínimo 6 caracteres.");

            // perfil
            if(administradorDTO.Perfil != "admin" && administradorDTO.Perfil != "leader") validacao.Mensagens.Add("O perfil deve ser 'admin' ou 'leader'.");

            return validacao;
        }
    }
}
