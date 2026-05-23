namespace PetFamily.Domain.Entities;

public class Lembrete
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataLembrete { get; set; }

    public int PetId { get; set; }
    public Pet Pet { get; set; } = null!;
}
