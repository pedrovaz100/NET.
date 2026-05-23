using PetFamily.Domain.Entities;

namespace PetFamily.Application.Interfaces.Repositories;

public interface IPetRepository : IRepository<Pet>
{
    Task<IEnumerable<Pet>> GetByTutorIdAsync(int tutorId);
    Task<IEnumerable<Pet>> GetByEspecieAsync(string especie);
    Task<IEnumerable<Pet>> GetByRacaAsync(string raca);
    Task<Pet?> GetByIdWithTutorAsync(int id);
    Task<IEnumerable<Pet>> GetAllWithTutorAsync();
}
