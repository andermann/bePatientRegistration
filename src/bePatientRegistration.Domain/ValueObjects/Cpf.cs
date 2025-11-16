using bePatientRegistration.Domain.Exceptions;

namespace bePatientRegistration.Domain.ValueObjects
{
    public class Cpf
    {
        // Armazena somente dígitos
        public string Value { get; private set; }

        private Cpf() { }

        public Cpf(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("CPF não pode ser vazio.");

            var normalized = Normalize(value);

            if (!IsValid(normalized))
                throw new DomainException("CPF inválido.");

            Value = normalized;
        }

        public static string Normalize(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        public static bool IsValid(string value)
        {
            var cpf = Normalize(value);

            if (cpf.Length != 11)
                return false;

            // Rejeita CPFs com todos os dígitos iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Calcula dígitos verificadores
            int[] multipliers1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliers2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += (tempCpf[i] - '0') * multipliers1[i];

            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += firstDigit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += (tempCpf[i] - '0') * multipliers2[i];

            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;

            string checkDigits = $"{firstDigit}{secondDigit}";

            return cpf.EndsWith(checkDigits);
        }

        public override string ToString() => Value;
    }
}
