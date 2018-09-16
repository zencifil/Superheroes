using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Superheroes.Contracts.Request;

namespace Superheroes.Handler
{
    public interface IHandler<T>
    {
        Task<IActionResult> HandleAsync(T request);
    }

    public class BattleRequestHandler : IHandler<BattleRequest>
    {
        private readonly IValidator _validator;

        public BattleRequestHandler(IValidator validator)
        {
            _validator = validator;
        }

        public Task<IActionResult> HandleAsync(BattleRequest request)
        {
            try
            {
                var validationResult = _validator.Validate(request);
                if (validationResult.Errors != null && validationResult.Errors.Count > 0)
                {
                    var badRequest = new BadRequestObjectResult(validationResult.Errors);
                    return Task.FromResult<IActionResult>(badRequest);
                }

                //TODO add data provider and do actual job
                var winner = "";
                return Task.FromResult<IActionResult>(new OkObjectResult(winner));
            }
            catch (System.Exception ex)
            {
                return Task.FromException<IActionResult>(ex);
            }
        }
    }
}
