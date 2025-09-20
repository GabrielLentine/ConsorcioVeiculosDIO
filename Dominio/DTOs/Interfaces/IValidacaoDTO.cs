using ConsorcioVeiculos.Dominio.ModelViews;

namespace ConsorcioVeiculos.Dominio.DTOs.Interfaces;
public interface IValidacaoDTO<TDto>
{
    public ErrosDeValidacao Validar(TDto dto);
}
