using PetFamily.Domain.Entities;

namespace PetFamily.Application.Interfaces.Repositories;

public interface ITutorRepository : IRepository<Tutor>
{
    Task<Tutor?> GetByEmailAsync(string email);
    Task<IEnumerable<Tutor>> SearchByNomeAsync(string nome);
}
