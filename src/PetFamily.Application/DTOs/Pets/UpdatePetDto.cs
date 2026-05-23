using System.ComponentModel.DataAnnotations;

namespace PetFamily.Application.DTOs.Pets;

public class UpdatePetDto
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Espécie é obrigatória.")]
    [MaxLength(50)]
    public string Especie { get; set; } = string.Empty;

    [Required(ErrorMessage = "Raça é obrigatória.")]
    [MaxLength(100)]
    public string Raca { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
    public DateTime DataNascimento { get; set; }

    [Required(ErrorMessage = "TutorId é obrigatório.")]
    public int TutorId { get; set; }
}
