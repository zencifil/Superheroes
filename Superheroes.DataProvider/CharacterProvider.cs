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
        Task<Character> GetCharacter(string name, string type);
        Task<IEnumerable<Character>> GetCharacters();
    }

    public class CharacterProvider : ICharacterProvider
    {
        const string CharactersUri = "https://s3.eu-west-2.amazonaws.com/build-circle/characters.json";
        readonly HttpClient _client = new HttpClient();
        IEnumerable<Character> _characters;

        public async Task<Character> GetCharacter(string name, string type)
        {
            if (_characters == null)
                _characters = await GetCharacters();
            return _characters.FirstOrDefault(c => c.Name == name && c.Type == type);
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
