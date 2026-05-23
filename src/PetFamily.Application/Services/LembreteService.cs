using PetFamily.Application.DTOs.Lembretes;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Exceptions;

namespace PetFamily.Application.Services;

public class LembreteService : ILembreteService
{
    private readonly ILembreteRepository _repository;
    private readonly IPetRepository _petRepository;

    public LembreteService(ILembreteRepository repository, IPetRepository petRepository)
    {
        _repository = repository;
        _petRepository = petRepository;
    }

    public async Task<IEnumerable<LembreteDto>> GetAllAsync()
    {
        var lembretes = await _repository.GetAllWithPetAsync();
        return lembretes.Select(MapToDto);
    }

    public async Task<LembreteDto?> GetByIdAsync(int id)
    {
        var lembrete = await _repository.GetByIdWithPetAsync(id);
        return lembrete is null ? null : MapToDto(lembrete);
    }

    public async Task<IEnumerable<LembreteDto>> GetByPetIdAsync(int petId)
    {
        var lembretes = await _repository.GetByPetIdAsync(petId);
        return lembretes.Select(MapToDto);
    }

    public async Task<IEnumerable<LembreteDto>> GetByDataAsync(DateTime data)
    {
        var lembretes = await _repository.GetByDataAsync(data);
        return lembretes.Select(MapToDto);
    }

    public async Task<IEnumerable<LembreteDto>> GetProximosAsync(int dias)
    {
        var lembretes = await _repository.GetProximosAsync(dias);
        return lembretes.Select(MapToDto);
    }

    public async Task<LembreteDto> CreateAsync(CreateLembreteDto dto)
    {
        var petExiste = await _petRepository.GetByIdAsync(dto.PetId);
        if (petExiste is null)
            throw new BusinessException($"Pet com ID {dto.PetId} não encontrado. Não é possível cadastrar o lembrete.");

        var lembrete = new Lembrete
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataLembrete = dto.DataLembrete,
            PetId = dto.PetId
        };

        await _repository.AddAsync(lembrete);
        await _repository.SaveChangesAsync();

        var created = await _repository.GetByIdWithPetAsync(lembrete.Id)
            ?? throw new InvalidOperationException($"Falha ao recuperar o lembrete {lembrete.Id} após criação.");

        return MapToDto(created);
    }

    public async Task<LembreteDto?> UpdateAsync(int id, UpdateLembreteDto dto)
    {
        var lembrete = await _repository.GetByIdAsync(id);
        if (lembrete is null) return null;

        var petExiste = await _petRepository.GetByIdAsync(dto.PetId);
        if (petExiste is null)
            throw new BusinessException($"Pet com ID {dto.PetId} não encontrado.");

        lembrete.Titulo = dto.Titulo;
        lembrete.Descricao = dto.Descricao;
        lembrete.DataLembrete = dto.DataLembrete;
        lembrete.PetId = dto.PetId;

        _repository.Update(lembrete);
        await _repository.SaveChangesAsync();

        var updated = await _repository.GetByIdWithPetAsync(lembrete.Id)
            ?? throw new InvalidOperationException($"Falha ao recuperar o lembrete {lembrete.Id} após atualização.");

        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lembrete = await _repository.GetByIdAsync(id);
        if (lembrete is null) return false;

        _repository.Delete(lembrete);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static LembreteDto MapToDto(Lembrete lembrete) => new()
    {
        Id = lembrete.Id,
        Titulo = lembrete.Titulo,
        Descricao = lembrete.Descricao,
        DataLembrete = lembrete.DataLembrete,
        PetId = lembrete.PetId,
        NomePet = lembrete.Pet?.Nome ?? string.Empty
    };
}
