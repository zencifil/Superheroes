using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Superheroes.Contracts.Request;
using Superheroes.Handler;

namespace Superheroes.Controllers
{
    [Route("battle")]
    public class BattleController : Controller
    {
        readonly IHandler<BattleRequest> _battleRequestHandler;

        public BattleController(IHandler<BattleRequest> battleRequestHandler)
        {
            _battleRequestHandler = battleRequestHandler;
        }

        public async Task<IActionResult> Get(string hero, string villain)
        {
            return await _battleRequestHandler.HandleAsync(new BattleRequest { Hero = hero, Villain = villain });
        }
    }
}