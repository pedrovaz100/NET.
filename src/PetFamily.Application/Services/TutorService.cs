using PetFamily.Application.DTOs.Tutores;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;

namespace PetFamily.Application.Services;

public class TutorService : ITutorService
{
    private readonly ITutorRepository _repository;

    public TutorService(ITutorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TutorDto>> GetAllAsync()
    {
        var tutores = await _repository.GetAllAsync();
        return tutores.Select(MapToDto);
    }

    public async Task<TutorDto?> GetByIdAsync(int id)
    {
        var tutor = await _repository.GetByIdAsync(id);
        return tutor is null ? null : MapToDto(tutor);
    }

    public async Task<TutorDto?> GetByEmailAsync(string email)
    {
        var tutor = await _repository.GetByEmailAsync(email);
        return tutor is null ? null : MapToDto(tutor);
    }

    public async Task<IEnumerable<TutorDto>> SearchByNomeAsync(string nome)
    {
        var tutores = await _repository.SearchByNomeAsync(nome);
        return tutores.Select(MapToDto);
    }

    public async Task<TutorDto> CreateAsync(CreateTutorDto dto)
    {
        var tutor = new Tutor
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone
        };

        await _repository.AddAsync(tutor);
        await _repository.SaveChangesAsync();
        return MapToDto(tutor);
    }

    public async Task<TutorDto?> UpdateAsync(int id, UpdateTutorDto dto)
    {
        var tutor = await _repository.GetByIdAsync(id);
        if (tutor is null) return null;

        tutor.Nome = dto.Nome;
        tutor.Email = dto.Email;
        tutor.Telefone = dto.Telefone;

        _repository.Update(tutor);
        await _repository.SaveChangesAsync();
        return MapToDto(tutor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tutor = await _repository.GetByIdAsync(id);
        if (tutor is null) return false;

        _repository.Delete(tutor);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static TutorDto MapToDto(Tutor tutor) => new()
    {
        Id = tutor.Id,
        Nome = tutor.Nome,
        Email = tutor.Email,
        Telefone = tutor.Telefone
    };
}
