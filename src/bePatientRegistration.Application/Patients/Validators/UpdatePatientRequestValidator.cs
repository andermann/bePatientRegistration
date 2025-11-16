using bePatientRegistration.Application.Patients.Dtos;
using FluentValidation;

namespace bePatientRegistration.Application.Patients.Validators
{
    public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
    {
        public UpdatePatientRequestValidator()
        {
            Include(new CreatePatientRequestValidator());

            // se quiser alguma regra específica para update, adiciona aqui
        }
    }
}
