using PetFamily.Application.DTOs.Lembretes;

namespace PetFamily.Application.Interfaces;

public interface ILembreteService
{
    Task<IEnumerable<LembreteDto>> GetAllAsync();
    Task<LembreteDto?> GetByIdAsync(int id);
    Task<IEnumerable<LembreteDto>> GetByPetIdAsync(int petId);
    Task<IEnumerable<LembreteDto>> GetByDataAsync(DateTime data);
    Task<IEnumerable<LembreteDto>> GetProximosAsync(int dias);
    Task<LembreteDto> CreateAsync(CreateLembreteDto dto);
    Task<LembreteDto?> UpdateAsync(int id, UpdateLembreteDto dto);
    Task<bool> DeleteAsync(int id);
}
