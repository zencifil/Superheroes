using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Superheroes.DataProvider;
using Superheroes.Domain.Entities;

namespace Superheroes.Tests
{
    public class FakeCharactersProvider : ICharacterProvider
    {
        IEnumerable<Character> _characters;
        
        public void FakeResponse(IEnumerable<Character> characters)
        {
            _characters = characters;
        }

        public Task<Character> GetCharacter(string name, string type)
        {
            return Task.FromResult(_characters.First(c => c.Name == name && c.Type == type));
        }

        public Task<IEnumerable<Character>> GetCharacters()
        {
            return Task.FromResult(_characters);
        }
    }
}
