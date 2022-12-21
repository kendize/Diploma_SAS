using FluentValidation;
using SAS.DTO;


namespace SAS.Validators
{
    public class AuthenticationRequestDTOValidator : AbstractValidator<AuthenticationRequestDTO>
    {
        public AuthenticationRequestDTOValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Email).NotNull().MinimumLength(5);
        }
    }
}
