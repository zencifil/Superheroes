using Superheroes.Contracts.Request;
using Superheroes.Validator;
using Xunit;

namespace Superheroes.Tests
{
    public class BattleRequestValidatorShould
    {
        readonly BattleRequestValidator _sut;

        public BattleRequestValidatorShould()
        {
            _sut = new BattleRequestValidator();
        }

        [Fact]
        public void ReturnError_WhenHeroNameIsEmpty()
        {
            var request = new BattleRequest { Hero = string.Empty, Villain = "villain" };
            var validationResult = _sut.Validate(request);

            Assert.True(validationResult.Errors.Count > 0);
        }

        [Fact]
        public void ReturnError_WhenVillainNameIsEmpty()
        {
            var request = new BattleRequest { Hero = "hero", Villain = string.Empty };
            var validationResult = _sut.Validate(request);

            Assert.True(validationResult.Errors.Count > 0);
        }

        [Fact]
        public void Validate_WhenRequestIsValid()
        {
            var request = new BattleRequest { Hero = "hero", Villain = "villain" };
            var validationResult = _sut.Validate(request);

            Assert.True(validationResult.Errors.Count == 0);
        }
    }
}
