namespace PetFamily.Application.DTOs.Consultas;

public class ConsultaDto
{
    public int Id { get; set; }
    public DateTime DataConsulta { get; set; }
    public string Observacoes { get; set; } = string.Empty;
    public int PetId { get; set; }
    public string NomePet { get; set; } = string.Empty;
}
