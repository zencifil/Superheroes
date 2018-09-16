using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Superheroes.Contracts.Request;
using Superheroes.DataProvider;
using Superheroes.Domain.Entities;

namespace Superheroes.Handler
{
    public interface IHandler<T>
    {
        Task<IActionResult> HandleAsync(T request);
    }

    public class BattleRequestHandler : IHandler<BattleRequest>
    {
        readonly IValidator _validator;
        readonly ICharacterProvider _characterProvider;

        public BattleRequestHandler(IValidator validator, ICharacterProvider characterProvider)
        {
            _validator = validator;
            _characterProvider = characterProvider;
        }

        public async Task<IActionResult> HandleAsync(BattleRequest request)
        {
            try
            {
                var validationResult = _validator.Validate(request);
                if (validationResult.Errors != null && validationResult.Errors.Count > 0)
                {
                    var badRequest = new BadRequestObjectResult(validationResult.Errors);
                    return await Task.FromResult<IActionResult>(badRequest);
                }

                var hero = await _characterProvider.GetCharacter(request.Hero);
                var villain = await _characterProvider.GetCharacter(request.Villain);
                var winner = Battle(hero, villain);

                return await Task.FromResult<IActionResult>(new OkObjectResult(winner));
            }
            catch (System.Exception ex)
            {
                return await Task.FromException<IActionResult>(ex);
            }
        }

        Character Battle(Character hero, Character villain)
        {
            if (!string.IsNullOrEmpty(hero.Weakness) && hero.Weakness == villain.Name)
            {
                hero.Score -= 1.0;
            }

            //for goodness sake, if the scores are equal hero wins...
            var winner = hero.Score >= villain.Score ? hero : villain;
            return winner;
        }
    }
}
