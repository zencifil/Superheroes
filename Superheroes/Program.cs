using FluentValidation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Superheroes.DataProvider;
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
                });
    }
}
