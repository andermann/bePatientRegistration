using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.Infrastructure.Repositories
{
    public class HealthPlanRepository : IHealthPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public HealthPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.HealthPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<HealthPlan>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.HealthPlans
                .AsNoTracking()
                .OrderBy(h => h.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(HealthPlan healthPlan, CancellationToken cancellationToken = default)
        {
            await _context.HealthPlans.AddAsync(healthPlan, cancellationToken);
        }

        public Task UpdateAsync(HealthPlan healthPlan, CancellationToken cancellationToken = default)
        {
            _context.HealthPlans.Update(healthPlan);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.HealthPlans.AnyAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.HealthPlans.AnyAsync(h => h.Name == name, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
