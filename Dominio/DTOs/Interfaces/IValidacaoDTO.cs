using ConsorcioVeiculos.Dominio.ModelViews;

namespace ConsorcioVeiculos.Dominio.DTOs.Interfaces;
public interface IValidacaoDTO
{
    public ErrosDeValidacao Validar(VeiculoDTO veiculoDTO);
}
