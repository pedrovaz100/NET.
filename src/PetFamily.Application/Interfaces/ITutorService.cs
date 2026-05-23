using PetFamily.Application.DTOs.Tutores;

namespace PetFamily.Application.Interfaces;

public interface ITutorService
{
    Task<IEnumerable<TutorDto>> GetAllAsync();
    Task<TutorDto?> GetByIdAsync(int id);
    Task<TutorDto?> GetByEmailAsync(string email);
    Task<IEnumerable<TutorDto>> SearchByNomeAsync(string nome);
    Task<TutorDto> CreateAsync(CreateTutorDto dto);
    Task<TutorDto?> UpdateAsync(int id, UpdateTutorDto dto);
    Task<bool> DeleteAsync(int id);
}
