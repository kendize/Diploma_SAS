using FluentValidation;
using SAS.DTO;

namespace SAS.Validators

{
    public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
    {
        public RegistrationDTOValidator()
        {
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Age).NotNull();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull().MinimumLength(5);
        }
    }
}
