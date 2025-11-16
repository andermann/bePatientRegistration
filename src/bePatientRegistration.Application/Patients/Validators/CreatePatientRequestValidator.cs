using bePatientRegistration.Application.Patients.Dtos;
using FluentValidation;

namespace bePatientRegistration.Application.Patients.Validators
{
    public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
    {
        public CreatePatientRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres.")
                .MaximumLength(100).WithMessage("Nome não pode ultrapassar 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Sobrenome é obrigatório.")
                .MinimumLength(3).WithMessage("Sobrenome deve ter pelo menos 3 caracteres.")
                .MaximumLength(100).WithMessage("Sobrenome não pode ultrapassar 100 caracteres.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
                .LessThanOrEqualTo(DateTime.Today)
                    .WithMessage("Data de nascimento não pode ser futura.")
                .GreaterThan(DateTime.Today.AddYears(-120))
                    .WithMessage("Data de nascimento inválida.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Sexo inválido.");

            RuleFor(x => x.Cpf)
                .Must(cpf => string.IsNullOrWhiteSpace(cpf) || cpf!.Length == 11)
                .WithMessage("CPF deve ter 11 dígitos numéricos.");

            RuleFor(x => x.Rg)
                .NotEmpty().WithMessage("RG é obrigatório.")
                .MinimumLength(3).MaximumLength(20);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(200);

            RuleFor(x => x.MobilePhone)
                .NotEmpty().WithMessage("Celular é obrigatório.")
                .Matches(@"^\d{10,11}$").WithMessage("Celular deve conter DDD + número (10 ou 11 dígitos).");

            RuleFor(x => x.LandlinePhone)
                .Must(p => string.IsNullOrWhiteSpace(p) || System.Text.RegularExpressions.Regex.IsMatch(p, @"^\d{10,11}$"))
                .WithMessage("Telefone fixo deve conter DDD + número (10 ou 11 dígitos).");

            RuleFor(x => x.HealthPlanId)
                .NotEmpty().WithMessage("Convênio é obrigatório.");

            RuleFor(x => x.HealthPlanCardNumber)
                .NotEmpty().WithMessage("Número da carteirinha é obrigatório.")
                .MinimumLength(3).MaximumLength(50);

            RuleFor(x => x.HealthPlanCardExpirationMonth)
                .InclusiveBetween(1, 12)
                .WithMessage("Mês de validade deve estar entre 1 e 12.");

            RuleFor(x => x.HealthPlanCardExpirationYear)
                .GreaterThanOrEqualTo(DateTime.Today.Year)
                .WithMessage("Ano de validade não pode ser menor que o ano atual.");
        }
    }
}
