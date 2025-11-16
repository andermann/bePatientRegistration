namespace bePatientRegistration.Domain.Entities
{
    public class HealthPlan
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<Patient> _patients = new();
        public IReadOnlyCollection<Patient> Patients => _patients.AsReadOnly();

        private HealthPlan() { }

        public HealthPlan(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
    }
}
