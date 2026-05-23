using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Infrastructure.Data;

namespace PetFamily.Infrastructure.Repositories;

public class LembreteRepository : Repository<Lembrete>, ILembreteRepository
{
    public LembreteRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Lembrete>> GetByPetIdAsync(int petId) =>
        await _dbSet.Include(l => l.Pet).Where(l => l.PetId == petId).ToListAsync();

    public async Task<IEnumerable<Lembrete>> GetByDataAsync(DateTime data) =>
        await _dbSet.Include(l => l.Pet).Where(l => l.DataLembrete.Date == data.Date).ToListAsync();

    public async Task<IEnumerable<Lembrete>> GetProximosAsync(int dias)
    {
        var limite = DateTime.Now.AddDays(dias);
        return await _dbSet.Include(l => l.Pet)
                           .Where(l => l.DataLembrete >= DateTime.Now && l.DataLembrete <= limite)
                           .OrderBy(l => l.DataLembrete)
                           .ToListAsync();
    }

    public async Task<Lembrete?> GetByIdWithPetAsync(int id) =>
        await _dbSet.Include(l => l.Pet).FirstOrDefaultAsync(l => l.Id == id);

    public async Task<IEnumerable<Lembrete>> GetAllWithPetAsync() =>
        await _dbSet.Include(l => l.Pet).ToListAsync();
}
