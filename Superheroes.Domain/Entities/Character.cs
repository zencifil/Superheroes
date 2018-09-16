namespace Superheroes.Domain.Entities
{
    public class Character
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public string Type { get; set; }
        public Character Weakness { get; set; }
    }
}
