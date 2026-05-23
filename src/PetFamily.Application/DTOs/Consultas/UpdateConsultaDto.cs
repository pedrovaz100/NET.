using System.ComponentModel.DataAnnotations;

namespace PetFamily.Application.DTOs.Consultas;

public class UpdateConsultaDto
{
    [Required(ErrorMessage = "Data da consulta é obrigatória.")]
    public DateTime DataConsulta { get; set; }

    [MaxLength(1000)]
    public string Observacoes { get; set; } = string.Empty;

    [Required(ErrorMessage = "PetId é obrigatório.")]
    public int PetId { get; set; }
}
