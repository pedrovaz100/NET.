using PetFamily.Application.DTOs.Consultas;

namespace PetFamily.Application.Interfaces;

public interface IConsultaService
{
    Task<IEnumerable<ConsultaDto>> GetAllAsync();
    Task<ConsultaDto?> GetByIdAsync(int id);
    Task<IEnumerable<ConsultaDto>> GetByPetIdAsync(int petId);
    Task<IEnumerable<ConsultaDto>> GetFuturasAsync();
    Task<IEnumerable<ConsultaDto>> GetByPeriodoAsync(DateTime inicio, DateTime fim);
    Task<ConsultaDto> CreateAsync(CreateConsultaDto dto);
    Task<ConsultaDto?> UpdateAsync(int id, UpdateConsultaDto dto);
    Task<bool> DeleteAsync(int id);
}
