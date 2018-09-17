using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Superheroes.Contracts.Request;
using Superheroes.Controllers;
using Superheroes.Handler;
using Xunit;

namespace Superheroes.Tests
{
    public class BattleControllerShould
    {
        readonly Mock<IHandler<BattleRequest>> _battleHandler;

        public BattleControllerShould()
        {
            _battleHandler = new Mock<IHandler<BattleRequest>>();
        }

        [Fact]
        public void ReturnBadRequest_WhenHeroNameOrVillainNameIsEmpty()
        {
            _battleHandler.Setup(h => h.HandleAsync(It.IsAny<BattleRequest>()))
                          .Returns(Task.FromResult<IActionResult>(new BadRequestObjectResult("")))
                          .Verifiable();

            var controller = new BattleController(_battleHandler.Object);
            var responseForNoHero = controller.Get(string.Empty, "VillainName").GetAwaiter().GetResult();
            responseForNoHero.Should().BeOfType(typeof(BadRequestObjectResult));

            var responseForNoVillain = controller.Get("hero", string.Empty).GetAwaiter().GetResult();
            responseForNoVillain.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void ReturnOk_WhenRequestIsSuccessfullyProcessed()
        {
            _battleHandler.Setup(h => h.HandleAsync(It.IsAny<BattleRequest>()))
                          .Returns(Task.FromResult<IActionResult>(new OkObjectResult("")))
                          .Verifiable();

            var controller = new BattleController(_battleHandler.Object);
            var response = controller.Get("hero", "villain").GetAwaiter().GetResult();
            response.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
