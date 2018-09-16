using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Superheroes.DataProvider;
using System.Collections.Generic;
using Superheroes.Domain.Entities;

namespace Superheroes.Tests
{
    public class BattleTests
    {
        [Fact]
        public async Task CanGetHeros()
        {
            var charactersProvider = new FakeCharactersProvider();

            var startup = new WebHostBuilder()
                            .UseStartup<Startup>()
                            .ConfigureServices(x =>
                            {
                                x.AddSingleton<ICharacterProvider>(charactersProvider);
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
