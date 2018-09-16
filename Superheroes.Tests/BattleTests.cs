using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Superheroes.Contracts.Request;
using Superheroes.DataProvider;
using Superheroes.Domain.Entities;
using Superheroes.Handler;
using Superheroes.Validator;
using Xunit;

namespace Superheroes.Tests
{
    public class BattleTests
    {
        [Fact]
        public async Task CanGetHeros()
        {
            var charactersProvider = new FakeCharactersProvider();
            var validator = new BattleRequestValidator();

            var startup = new WebHostBuilder()
                            .UseStartup<Startup>()
                            .ConfigureServices(x =>
                            {
                                x.AddSingleton<ICharacterProvider>(charactersProvider);
                                x.AddSingleton<IValidator>(validator);
                                x.AddScoped<IHandler<BattleRequest>, BattleRequestHandler>();
                            });
            var testServer = new TestServer(startup);
            var client = testServer.CreateClient();

            charactersProvider.FakeResponse(new List<Character>
            {
                new Character
                {
                    Name = "Batman",
                    Score = 8.3,
                    Type = "hero"
                },
                new Character
                {
                    Name = "Joker",
                    Score = 8.2,
                    Type = "villain"
                }
            });

            var response = await client.GetAsync("battle?hero=Batman&villain=Joker");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseJson);

            responseObject.Value<string>("name").Should().Be("Batman");
        }
    }
}
