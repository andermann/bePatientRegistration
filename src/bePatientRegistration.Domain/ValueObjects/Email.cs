using System.Text.RegularExpressions;
using bePatientRegistration.Domain.Exceptions;

namespace bePatientRegistration.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        private Email() { }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("E-mail não pode ser vazio.");

            if (!IsValid(value))
                throw new DomainException("E-mail inválido.");

            Value = value.Trim();
        }

        public static bool IsValid(string value)
        {
            // Regex simples, suficiente pro desafio
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
        }

        public override string ToString() => Value;
    }
}
