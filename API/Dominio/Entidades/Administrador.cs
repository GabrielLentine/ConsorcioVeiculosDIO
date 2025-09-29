using ConsorcioVeiculos.Dominio.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsorcioVeiculos.Dominio.Entidades;
public class Administrador
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default;

    [Required(ErrorMessage = "O e-mail é um campo obrigatório!")]
    [StringLength(255)]
    public string? Email { get; set; } = default;

    [Required(ErrorMessage = "A senha é um campo obrigatório!")]
    [StringLength(50)]
    public string? Senha { get; set; } = default;

    [Required(ErrorMessage = "O prefil é um campo obrigatório")]
    [StringLength(10)]
    public Perfil Perfil { get; set; } = default;
}
