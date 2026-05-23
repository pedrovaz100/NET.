namespace PetFamily.Domain.Entities;

public class Pet
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raca { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }

    public int TutorId { get; set; }
    public Tutor Tutor { get; set; } = null!;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    public ICollection<Lembrete> Lembretes { get; set; } = new List<Lembrete>();
}
