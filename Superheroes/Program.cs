using FluentValidation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Superheroes.Contracts.Request;
using Superheroes.DataProvider;
using Superheroes.Handler;
using Superheroes.Validator;

namespace Superheroes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(x => 
                {
                    x.AddSingleton<ICharacterProvider, CharacterProvider>();
                    x.AddSingleton<IValidator, BattleRequestValidator>();
                    x.AddScoped<IHandler<BattleRequest>, BattleRequestHandler>();
                });
    }
}
