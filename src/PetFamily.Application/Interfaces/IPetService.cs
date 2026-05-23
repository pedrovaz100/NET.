using PetFamily.Application.DTOs.Pets;

namespace PetFamily.Application.Interfaces;

public interface IPetService
{
    Task<IEnumerable<PetDto>> GetAllAsync();
    Task<PetDto?> GetByIdAsync(int id);
    Task<IEnumerable<PetDto>> GetByTutorIdAsync(int tutorId);
    Task<IEnumerable<PetDto>> GetByEspecieAsync(string especie);
    Task<IEnumerable<PetDto>> GetByRacaAsync(string raca);
    Task<PetDto> CreateAsync(CreatePetDto dto);
    Task<PetDto?> UpdateAsync(int id, UpdatePetDto dto);
    Task<bool> DeleteAsync(int id);
}
