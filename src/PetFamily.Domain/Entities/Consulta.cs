namespace PetFamily.Domain.Entities;

public class Consulta
{
    public int Id { get; set; }
    public DateTime DataConsulta { get; set; }
    public string Observacoes { get; set; } = string.Empty;

    public int PetId { get; set; }
    public Pet Pet { get; set; } = null!;
}
