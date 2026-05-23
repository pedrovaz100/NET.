using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Entities;
using PetFamily.Infrastructure.Data;

namespace PetFamily.Infrastructure.Repositories;

public class TutorRepository : Repository<Tutor>, ITutorRepository
{
    public TutorRepository(AppDbContext context) : base(context) { }

    public async Task<Tutor?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());

    public async Task<IEnumerable<Tutor>> SearchByNomeAsync(string nome) =>
        await _dbSet.Where(t => t.Nome.ToLower().Contains(nome.ToLower())).ToListAsync();
}
