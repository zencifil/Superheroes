using FluentValidation;
using Superheroes.Contracts.Request;

namespace Superheroes.Validator
{
    public class BattleRequestValidator : AbstractValidator<BattleRequest>
    {
        public BattleRequestValidator()
        {
            RuleFor(r => r.Hero).NotEmpty().WithMessage("Hero name cannot be empty.");
            RuleFor(r => r.Villain).NotEmpty().WithMessage("Villain name cannot be empty");
        }

    }
}
