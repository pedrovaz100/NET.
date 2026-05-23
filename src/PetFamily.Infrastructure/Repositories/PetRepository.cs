using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Infrastructure.Data;

namespace PetFamily.Infrastructure.Repositories;

public class PetRepository : Repository<Pet>, IPetRepository
{
    public PetRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Pet>> GetByTutorIdAsync(int tutorId) =>
        await _dbSet.Include(p => p.Tutor).Where(p => p.TutorId == tutorId).ToListAsync();

    public async Task<IEnumerable<Pet>> GetByEspecieAsync(string especie) =>
        await _dbSet.Include(p => p.Tutor).Where(p => p.Especie.ToLower() == especie.ToLower()).ToListAsync();

    public async Task<IEnumerable<Pet>> GetByRacaAsync(string raca) =>
        await _dbSet.Include(p => p.Tutor).Where(p => p.Raca.ToLower().Contains(raca.ToLower())).ToListAsync();

    public async Task<Pet?> GetByIdWithTutorAsync(int id) =>
        await _dbSet.Include(p => p.Tutor).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Pet>> GetAllWithTutorAsync() =>
        await _dbSet.Include(p => p.Tutor).ToListAsync();
}
