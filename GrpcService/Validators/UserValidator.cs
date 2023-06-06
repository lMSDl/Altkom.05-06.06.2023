using FluentValidation;
using GrpcService.Protos.Users;

namespace GrpcService.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() {
            RuleFor(x => x.Id).GreaterThan(0);

        }
    }
}
