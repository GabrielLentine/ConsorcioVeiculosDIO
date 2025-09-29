using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsorcioVeiculos.Dominio.Entidades
{
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default;

        [Required(ErrorMessage = "O nome do veículo é um campo obrigatório!")]
        [StringLength(150)]
        public string? Nome { get; set; } = default;

        [Required(ErrorMessage = "A marca do veículo é um campo obrigatório!")]
        [StringLength(100)]
        public string? Marca { get; set; } = default;

        [Required(ErrorMessage = "O ano do veículo é um campo obrigatório!")]
        public int Ano { get; set; } = default;
    }
}
