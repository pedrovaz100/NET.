namespace PetFamily.Application.DTOs.Pets;

public class PetDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raca { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public int TutorId { get; set; }
    public string NomeTutor { get; set; } = string.Empty;
}
