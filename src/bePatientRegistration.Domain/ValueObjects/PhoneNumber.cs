using bePatientRegistration.Domain.Exceptions;

namespace bePatientRegistration.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; private set; }

        private PhoneNumber() { }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Telefone não pode ser vazio.");

            var normalized = Normalize(value);

            // Validação simples: mínimo 10 dígitos (DDD + número)
            if (normalized.Length < 10 || normalized.Length > 11)
                throw new DomainException("Telefone inválido.");

            Value = normalized;
        }

        public static string Normalize(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        public override string ToString() => Value;
    }
}
