using PetFamily.Domain.Entities;

namespace PetFamily.Application.Interfaces.Repositories;

public interface ILembreteRepository : IRepository<Lembrete>
{
    Task<IEnumerable<Lembrete>> GetByPetIdAsync(int petId);
    Task<IEnumerable<Lembrete>> GetByDataAsync(DateTime data);
    Task<IEnumerable<Lembrete>> GetProximosAsync(int dias);
    Task<Lembrete?> GetByIdWithPetAsync(int id);
    Task<IEnumerable<Lembrete>> GetAllWithPetAsync();
}
