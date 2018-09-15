using System.Collections.Generic;
using Superheroes.Contracts.Dto;

namespace Superheroes.Contracts.Response
{
    public class BattleResponse
    {
        public IEnumerable<CharacterDto> Characters { get; set; }
    }
}
