namespace PetFamily.Application.DTOs.Lembretes;

public class LembreteDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataLembrete { get; set; }
    public int PetId { get; set; }
    public string NomePet { get; set; } = string.Empty;
}
