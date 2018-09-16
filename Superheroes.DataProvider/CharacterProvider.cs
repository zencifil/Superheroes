using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Superheroes.Domain.Entities;

namespace Superheroes.DataProvider
{
    public interface ICharacterProvider
    {
        Task<Character> GetCharacter(string name);
        Task<IEnumerable<Character>> GetCharacters();
    }

    public class CharacterProvider : ICharacterProvider
    {
        const string CharactersUri = "https://s3.eu-west-2.amazonaws.com/build-circle/characters.json";
        readonly HttpClient _client = new HttpClient();

        public async Task<Character> GetCharacter(string name)
        {
            var response = await _client.GetAsync(CharactersUri);

            var responseJson = await response.Content.ReadAsStringAsync();
            var characterResponse = JsonConvert.DeserializeObject<CharacterResponse>(responseJson);

            return characterResponse.Items.First(c => c.Name == name);
        }

        public async Task<IEnumerable<Character>> GetCharacters()
        {
            var response = await _client.GetAsync(CharactersUri);

            var responseJson = await response.Content.ReadAsStringAsync();
            var characterResponse = JsonConvert.DeserializeObject<CharacterResponse>(responseJson);

            return characterResponse.Items;
        }
    }
}
