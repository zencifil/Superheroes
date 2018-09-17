using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Superheroes.Contracts.Request;
using Superheroes.Domain.Entities;
using Superheroes.Handler;
using Superheroes.Validator;
using Xunit;

namespace Superheroes.Tests
{
    public class BattleRequestHandlerShould
    {
        readonly IHandler<BattleRequest> _battleHandler;
        readonly BattleRequestValidator _validator;
        readonly FakeCharactersProvider _characterProvider;

        public BattleRequestHandlerShould()
        {
            _validator = new BattleRequestValidator();
            _characterProvider = new FakeCharactersProvider();
            _characterProvider.FakeResponse(new List<Character>
            {
                new Character { Name = "Batman", Score = 8.3, Type = "hero", Weakness = "Joker" },
                new Character { Name = "Joker", Score = 8.2, Type = "villain", Weakness = null },
                new Character { Name = "Superman", Score = 9.9, Type = "hero", Weakness = "Lex Luthor" },
                new Character { Name = "Lex Luthor", Score = 8.6, Type = "villain", Weakness = null }
            });
            _battleHandler = new BattleRequestHandler(_validator, _characterProvider);
        }

        [Fact]
        public async Task ReturnBadRequest_WhenNoHeroFoundWithGivenName()
        {
            var request = new BattleRequest { Hero = "He-man", Villain = "Joker" };
            var response = await _battleHandler.HandleAsync(request);

            var expectedResult = new BadRequestObjectResult("The hero you're looking for could not be found!");

            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Matches(((BadRequestObjectResult)response).Value.ToString(), expectedResult.Value.ToString());
        }

        [Fact]
        public async Task ReturnBadRequest_WhenNoVillainFoundWithGivenName()
        {
            var request = new BattleRequest { Hero = "Superman", Villain = "Skeletor" };
            var response = await _battleHandler.HandleAsync(request);

            var expectedResult = new BadRequestObjectResult("The villain you're looking for could not be found!");

            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Matches(((BadRequestObjectResult)response).Value.ToString(), expectedResult.Value.ToString());
        }

        [Fact]
        public async Task ReturnJokerAsWinner_WhenBattleWithBatman()
        {
            var request = new BattleRequest { Hero = "Batman", Villain = "Joker" };
            var response = await _battleHandler.HandleAsync(request);

            var expectedWinner = await _characterProvider.GetCharacter("Joker", "villain");

            Assert.IsType<OkObjectResult>(response);
            Assert.Same(((OkObjectResult)response).Value, expectedWinner);
        }

        [Fact]
        public async Task ReturnSupermanAsWinner_WhenBattleWithLexLuthor()
        {
            var request = new BattleRequest { Hero = "Superman", Villain = "Lex Luthor" };
            var response = await _battleHandler.HandleAsync(request);

            var expectedWinner = await _characterProvider.GetCharacter("Superman", "hero");

            Assert.IsType<OkObjectResult>(response);
            Assert.Same(((OkObjectResult)response).Value, expectedWinner);
        }

        [Fact]
        public async Task ReturnBadRequest_WhenBatmanChallengesSuperman()
        {
            var request = new BattleRequest { Hero = "Batman", Villain = "Superman" };
            var response = await _battleHandler.HandleAsync(request);

            var expectedResult = new BadRequestObjectResult("The villain you're looking for could not be found!");

            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Matches(((BadRequestObjectResult)response).Value.ToString(), expectedResult.Value.ToString());
        }
    }
}
