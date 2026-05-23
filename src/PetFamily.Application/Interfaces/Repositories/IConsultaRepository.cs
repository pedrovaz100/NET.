using PetFamily.Domain.Entities;

namespace PetFamily.Application.Interfaces.Repositories;

public interface IConsultaRepository : IRepository<Consulta>
{
    Task<IEnumerable<Consulta>> GetByPetIdAsync(int petId);
    Task<IEnumerable<Consulta>> GetFuturasAsync();
    Task<IEnumerable<Consulta>> GetByPeriodoAsync(DateTime inicio, DateTime fim);
    Task<Consulta?> GetByIdWithPetAsync(int id);
    Task<IEnumerable<Consulta>> GetAllWithPetAsync();
}
