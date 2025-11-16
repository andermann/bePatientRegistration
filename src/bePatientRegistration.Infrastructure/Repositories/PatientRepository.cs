using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .Include(p => p.HealthPlan)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .Include(p => p.HealthPlan)
                .AsNoTracking()
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            await _context.Patients.AddAsync(patient, cancellationToken);
        }

        public Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            _context.Patients.Update(patient);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            _context.Patients.Remove(patient);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Patients.AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .AnyAsync(p => p.Cpf != null && p.Cpf.Value == cpf, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
