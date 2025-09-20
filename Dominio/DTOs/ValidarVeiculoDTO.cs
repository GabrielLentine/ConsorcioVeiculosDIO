using ConsorcioVeiculos.Dominio.DTOs.Interfaces;
using ConsorcioVeiculos.Dominio.ModelViews;

namespace ConsorcioVeiculos.Dominio.DTOs;

public class ValidarVeiculoDTO : IValidacaoDTO<VeiculoDTO>
{
    public ErrosDeValidacao Validar(VeiculoDTO veiculoDTO)
    {
        var validacao = new ErrosDeValidacao();

        // nome
        if(string.IsNullOrEmpty(veiculoDTO.Nome)) validacao.Mensagens.Add("O nome do veículo é obrigatório.");
        if(!char.IsUpper(veiculoDTO.Nome[0])) validacao.Mensagens.Add("A primeira letra do nome do veículo deve ser maiúscula.");

        // marca
        if(string.IsNullOrEmpty(veiculoDTO.Marca)) validacao.Mensagens.Add("A marca do veículo é obrigatória.");
        if(!char.IsUpper(veiculoDTO.Marca[0])) validacao.Mensagens.Add("A primeira letra da marca do veículo deve ser maiúscula.");

        // ano
        if(veiculoDTO.Ano < 1950 || veiculoDTO.Ano > DateTime.Now.Year + 1) validacao.Mensagens.Add($"O ano do veículo deve ser entre 1950 e {DateTime.Now.Year + 1}.");

        return validacao;
    }
}
