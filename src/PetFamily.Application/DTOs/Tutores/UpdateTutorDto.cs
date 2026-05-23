using System.ComponentModel.DataAnnotations;

namespace PetFamily.Application.DTOs.Tutores;

public class UpdateTutorDto
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório.")]
    [MaxLength(20)]
    public string Telefone { get; set; } = string.Empty;
}
