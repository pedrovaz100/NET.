using System.ComponentModel.DataAnnotations;

namespace PetFamily.Application.DTOs.Lembretes;

public class UpdateLembreteDto
{
    [Required(ErrorMessage = "Título é obrigatório.")]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data do lembrete é obrigatória.")]
    public DateTime DataLembrete { get; set; }

    [Required(ErrorMessage = "PetId é obrigatório.")]
    public int PetId { get; set; }
}
