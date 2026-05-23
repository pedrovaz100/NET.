using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Infrastructure.Data;

namespace PetFamily.Infrastructure.Repositories;

public class ConsultaRepository : Repository<Consulta>, IConsultaRepository
{
    public ConsultaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Consulta>> GetByPetIdAsync(int petId) =>
        await _dbSet.Include(c => c.Pet).Where(c => c.PetId == petId).ToListAsync();

    public async Task<IEnumerable<Consulta>> GetFuturasAsync() =>
        await _dbSet.Include(c => c.Pet).Where(c => c.DataConsulta >= DateTime.Now).OrderBy(c => c.DataConsulta).ToListAsync();

    public async Task<IEnumerable<Consulta>> GetByPeriodoAsync(DateTime inicio, DateTime fim) =>
        await _dbSet.Include(c => c.Pet).Where(c => c.DataConsulta >= inicio && c.DataConsulta <= fim).OrderBy(c => c.DataConsulta).ToListAsync();

    public async Task<Consulta?> GetByIdWithPetAsync(int id) =>
        await _dbSet.Include(c => c.Pet).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Consulta>> GetAllWithPetAsync() =>
        await _dbSet.Include(c => c.Pet).ToListAsync();
}
