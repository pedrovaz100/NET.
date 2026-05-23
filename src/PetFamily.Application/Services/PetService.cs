using PetFamily.Application.DTOs.Pets;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Exceptions;

namespace PetFamily.Application.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _repository;
    private readonly ITutorRepository _tutorRepository;

    public PetService(IPetRepository repository, ITutorRepository tutorRepository)
    {
        _repository = repository;
        _tutorRepository = tutorRepository;
    }

    public async Task<IEnumerable<PetDto>> GetAllAsync()
    {
        var pets = await _repository.GetAllWithTutorAsync();
        return pets.Select(MapToDto);
    }

    public async Task<PetDto?> GetByIdAsync(int id)
    {
        var pet = await _repository.GetByIdWithTutorAsync(id);
        return pet is null ? null : MapToDto(pet);
    }

    public async Task<IEnumerable<PetDto>> GetByTutorIdAsync(int tutorId)
    {
        var pets = await _repository.GetByTutorIdAsync(tutorId);
        return pets.Select(MapToDto);
    }

    public async Task<IEnumerable<PetDto>> GetByEspecieAsync(string especie)
    {
        var pets = await _repository.GetByEspecieAsync(especie);
        return pets.Select(MapToDto);
    }

    public async Task<IEnumerable<PetDto>> GetByRacaAsync(string raca)
    {
        var pets = await _repository.GetByRacaAsync(raca);
        return pets.Select(MapToDto);
    }

    public async Task<PetDto> CreateAsync(CreatePetDto dto)
    {
        var tutorExiste = await _tutorRepository.GetByIdAsync(dto.TutorId);
        if (tutorExiste is null)
            throw new BusinessException($"Tutor com ID {dto.TutorId} não encontrado. Não é possível cadastrar o pet.");

        var pet = new Pet
        {
            Nome = dto.Nome,
            Especie = dto.Especie,
            Raca = dto.Raca,
            DataNascimento = dto.DataNascimento,
            TutorId = dto.TutorId
        };

        await _repository.AddAsync(pet);
        await _repository.SaveChangesAsync();

        var created = await _repository.GetByIdWithTutorAsync(pet.Id)
            ?? throw new InvalidOperationException($"Falha ao recuperar o pet {pet.Id} após criação.");

        return MapToDto(created);
    }

    public async Task<PetDto?> UpdateAsync(int id, UpdatePetDto dto)
    {
        var pet = await _repository.GetByIdAsync(id);
        if (pet is null) return null;

        var tutorExiste = await _tutorRepository.GetByIdAsync(dto.TutorId);
        if (tutorExiste is null)
            throw new BusinessException($"Tutor com ID {dto.TutorId} não encontrado.");

        pet.Nome = dto.Nome;
        pet.Especie = dto.Especie;
        pet.Raca = dto.Raca;
        pet.DataNascimento = dto.DataNascimento;
        pet.TutorId = dto.TutorId;

        _repository.Update(pet);
        await _repository.SaveChangesAsync();

        var updated = await _repository.GetByIdWithTutorAsync(pet.Id)
            ?? throw new InvalidOperationException($"Falha ao recuperar o pet {pet.Id} após atualização.");

        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pet = await _repository.GetByIdAsync(id);
        if (pet is null) return false;

        _repository.Delete(pet);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static PetDto MapToDto(Pet pet) => new()
    {
        Id = pet.Id,
        Nome = pet.Nome,
        Especie = pet.Especie,
        Raca = pet.Raca,
        DataNascimento = pet.DataNascimento,
        TutorId = pet.TutorId,
        NomeTutor = pet.Tutor?.Nome ?? string.Empty
    };
}
