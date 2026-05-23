using PetFamily.Application.DTOs.Consultas;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Exceptions;

namespace PetFamily.Application.Services;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _repository;
    private readonly IPetRepository _petRepository;

    public ConsultaService(IConsultaRepository repository, IPetRepository petRepository)
    {
        _repository = repository;
        _petRepository = petRepository;
    }

    public async Task<IEnumerable<ConsultaDto>> GetAllAsync()
    {
        var consultas = await _repository.GetAllWithPetAsync();
        return consultas.Select(MapToDto);
    }

    public async Task<ConsultaDto?> GetByIdAsync(int id)
    {
        var consulta = await _repository.GetByIdWithPetAsync(id);
        return consulta is null ? null : MapToDto(consulta);
    }

    public async Task<IEnumerable<ConsultaDto>> GetByPetIdAsync(int petId)
    {
        var consultas = await _repository.GetByPetIdAsync(petId);
        return consultas.Select(MapToDto);
    }

    public async Task<IEnumerable<ConsultaDto>> GetFuturasAsync()
    {
        var consultas = await _repository.GetFuturasAsync();
        return consultas.Select(MapToDto);
    }

    public async Task<IEnumerable<ConsultaDto>> GetByPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var consultas = await _repository.GetByPeriodoAsync(inicio, fim);
        return consultas.Select(MapToDto);
    }

    public async Task<ConsultaDto> CreateAsync(CreateConsultaDto dto)
    {
        var petExiste = await _petRepository.GetByIdAsync(dto.PetId);
        if (petExiste is null)
            throw new BusinessException($"Pet com ID {dto.PetId} não encontrado. Não é possível cadastrar a consulta.");

        var consulta = new Consulta
        {
            DataConsulta = dto.DataConsulta,
            Observacoes = dto.Observacoes,
            PetId = dto.PetId
        };

        await _repository.AddAsync(consulta);
        await _repository.SaveChangesAsync();

        var created = await _repository.GetByIdWithPetAsync(consulta.Id)
            ?? throw new InvalidOperationException($"Falha ao recuperar a consulta {consulta.Id} após criação.");

        return MapToDto(created);
    }

    public async Task<ConsultaDto?> UpdateAsync(int id, UpdateConsultaDto dto)
    {
        var consulta = await _repository.GetByIdAsync(id);
        if (consulta is null) return null;

        var petExiste = await _petRepository.GetByIdAsync(dto.PetId);
        if (petExiste is null)
            throw new BusinessException($"Pet com ID {dto.PetId} não encontrado.");

        consulta.DataConsulta = dto.DataConsulta;
        consulta.Observacoes = dto.Observacoes;
        consulta.PetId = dto.PetId;

        _repository.Update(consulta);
        await _repository.SaveChangesAsync();

        var updated = await _repository.GetByIdWithPetAsync(consulta.Id)
            ?? throw new InvalidOperationException($"Falha ao recuperar a consulta {consulta.Id} após atualização.");

        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var consulta = await _repository.GetByIdAsync(id);
        if (consulta is null) return false;

        _repository.Delete(consulta);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static ConsultaDto MapToDto(Consulta consulta) => new()
    {
        Id = consulta.Id,
        DataConsulta = consulta.DataConsulta,
        Observacoes = consulta.Observacoes,
        PetId = consulta.PetId,
        NomePet = consulta.Pet?.Nome ?? string.Empty
    };
}
