using SQLite4Unity3d;

namespace Domain
{
    public class PokemonData
    {
        [PrimaryKey] public int PokedexNb { get; set; }
        public string NameFr { get; set; }
        public bool Owned { get; set; }
        public Language OwnedLanguage { get; set; }
        public CardRarity Rarity { get; set; }

        public PokemonData Clone()
        {
            return new PokemonData
            {
                PokedexNb = PokedexNb,
                NameFr = NameFr,
                Owned = Owned,
                OwnedLanguage = OwnedLanguage,
                Rarity = Rarity
            };
        }
    }
}