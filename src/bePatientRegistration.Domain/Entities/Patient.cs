using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;
using System.Threading;

namespace bePatientRegistration.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public Gender Gender { get; private set; }

        public Cpf? Cpf { get; private set; }                  // opcional
        public string Rg { get; private set; }
        public Uf UfRg { get; private set; }

        public Email Email { get; private set; }
        public PhoneNumber MobilePhone { get; private set; }
        public PhoneNumber? LandlinePhone { get; private set; }

        public Guid HealthPlanId { get; private set; }
        public HealthPlan HealthPlan { get; private set; }

        public string HealthPlanCardNumber { get; private set; }
        public int HealthPlanCardExpirationMonth { get; private set; }
        public int HealthPlanCardExpirationYear { get; private set; }

        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Patient() { }

        public Patient(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            string? cpf,
            string rg,
            Uf ufRg,
            string email,
            string mobilePhone,
            string? landlinePhone,
            Guid healthPlanId,
            string healthPlanCardNumber,
            int healthPlanCardExpirationMonth,
            int healthPlanCardExpirationYear)
        {
            Id = Guid.NewGuid();

            //// regra simples: não permitir CPF duplicado
            //if (!string.IsNullOrWhiteSpace(cpf))
            //{
            //    var normalizedCpf = cpf.Normalize(cpf);
            //    if (ExistsByCpfAsync(normalizedCpf))
            //        throw new DomainException("Já existe um paciente cadastrado com este CPF.");
            //}

            SetName(firstName, lastName);
            SetDateOfBirth(dateOfBirth);
            Gender = gender;

            if (!string.IsNullOrWhiteSpace(cpf))
                Cpf = new Cpf(cpf);

            Rg = string.IsNullOrWhiteSpace(rg)
                ? throw new DomainException("RG é obrigatório.")
                : rg.Trim();

            UfRg = ufRg;

            Email = new Email(email);
            MobilePhone = new PhoneNumber(mobilePhone);

            if (!string.IsNullOrWhiteSpace(landlinePhone))
                LandlinePhone = new PhoneNumber(landlinePhone);

            if (LandlinePhone == null && MobilePhone == null)
                throw new DomainException("Pelo menos um telefone deve ser informado.");

            HealthPlanId = healthPlanId;
            HealthPlanCardNumber = string.IsNullOrWhiteSpace(healthPlanCardNumber)
                ? throw new DomainException("Número da carteirinha é obrigatório.")
                : healthPlanCardNumber.Trim();

            SetCardExpiration(healthPlanCardExpirationMonth, healthPlanCardExpirationYear);

            ValidatePhones(mobilePhone, landlinePhone);

            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Sobrenome é obrigatório.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void SetDateOfBirth(DateTime date)
        {
            if (date > DateTime.Today)
                throw new DomainException("Data de nascimento não pode ser futura.");

            // opcional: validar idade mínima / máxima

            DateOfBirth = date.Date;
        }

        public void SetCardExpiration(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new DomainException("Mês de validade do convênio inválido.");

            if (year < DateTime.Today.Year ||
               (year == DateTime.Today.Year && month < DateTime.Today.Month))
            {
                throw new DomainException("Carteirinha do convênio está vencida.");
            }

            HealthPlanCardExpirationMonth = month;
            HealthPlanCardExpirationYear = year;
        }

        public static void ValidatePhones(string? mobilePhone, string? landlinePhone)
        {
            if (string.IsNullOrWhiteSpace(mobilePhone) && string.IsNullOrWhiteSpace(landlinePhone))
            {
                throw new DomainException("Informe pelo menos um telefone (celular ou fixo).");
            }
        }


        public void Update(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            string? cpf,
            string rg,
            Uf ufRg,
            string email,
            string mobilePhone,
            string? landlinePhone,
            Guid healthPlanId,
            string healthPlanCardNumber,
            int healthPlanCardExpirationMonth,
            int healthPlanCardExpirationYear)
        {
            SetName(firstName, lastName);
            SetDateOfBirth(dateOfBirth);
            Gender = gender;

            Cpf = !string.IsNullOrWhiteSpace(cpf) ? new Cpf(cpf) : null;

            Rg = string.IsNullOrWhiteSpace(rg)
                ? throw new DomainException("RG é obrigatório.")
                : rg.Trim();

            UfRg = ufRg;

            Email = new Email(email);
            MobilePhone = new PhoneNumber(mobilePhone);
            LandlinePhone = !string.IsNullOrWhiteSpace(landlinePhone)
                ? new PhoneNumber(landlinePhone)
                : null;

            HealthPlanId = healthPlanId;
            HealthPlanCardNumber = healthPlanCardNumber;
            SetCardExpiration(healthPlanCardExpirationMonth, healthPlanCardExpirationYear);

            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
