using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Domain;
using InfoLoader;
using TMPro;
using UnityEngine;

namespace App
{
    public class PokemonDisplay : MonoBehaviour
    {
        private const int MaxPokemon = 1025;
        
        [SerializeField] private DataEditScreen dataEditScreen;
        [SerializeField] private FileSaveScreen fileSaveScreen;
        
        [SerializeField] private RectTransform content;
        [SerializeField] private TMP_Text filterSurvivorsCount;

        [SerializeField] private CustomButton saveFileButton;
        
        
        
        private Dictionary<int, PokemonView> allViews = new ();
        private List<Predicate<PokemonData>> rarityFilters = new();
        private List<Predicate<PokemonData>> languageFilters = new();
        private List<Predicate<PokemonData>> ownershipFilters = new();
        private string nameSearch = "";
        
        private DatabaseManager databaseManager;


        private void Awake()
        {
            databaseManager = FindFirstObjectByType<DatabaseManager>();
            saveFileButton.DefineBehaviour(() => fileSaveScreen.Initialize(allViews.Values.ToList()));
        }

        private void Start()
        {
            FillPokemonViews();
            ResetDisplayWithFilters();
        }

        public void InitializePokemonView(PokemonData pokemonData)
        {
            var pokemonView = allViews.ContainsKey(pokemonData.PokedexNb) ? allViews[pokemonData.PokedexNb] : content.GetChild(pokemonData.PokedexNb - 1).GetComponent<PokemonView>();
            pokemonData.NameFr = pokemonView.nameFr.text;
            pokemonView.Initialize(pokemonData, () => dataEditScreen.Initialize(pokemonView));
            allViews.Add(pokemonData.PokedexNb, pokemonView);
        }
        
        private string RemoveDiacritics(string text) 
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            foreach (var c in from c in normalizedString let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) where unicodeCategory != UnicodeCategory.NonSpacingMark select c)
                stringBuilder.Append(c);

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        private IEnumerable<string> AddDashNamesVariables(string input) => new List<string> {"-", " ", ""}.Select(s => input.Replace("-", s));

        public void ResetDisplayWithFilters()
        {
            allViews.Values.ToList().ForEach(pv =>
            {
                var treatedNameSearch = RemoveDiacritics(nameSearch.ToLower());
                var parseSuccess = int.TryParse(nameSearch, out var nameSearchInt) && nameSearchInt is > 0 and <= MaxPokemon;
                var treatedPvName = RemoveDiacritics(pv.pokemonData.NameFr.ToLower());
                var nameFilter = AddDashNamesVariables(treatedPvName).Any(dnv => dnv.Contains(treatedNameSearch)) || (parseSuccess && nameSearchInt == pv.pokemonData.PokedexNb);
                var compoundRarityFilter = rarityFilters.Count == 0 || rarityFilters.Any(p => p.Invoke(pv.pokemonData));
                var compoundLanguageFilter = languageFilters.Count == 0 || languageFilters.Any(p => p.Invoke(pv.pokemonData));
                var compoundOwnershipFilter = ownershipFilters.Count == 0 || ownershipFilters.Any(p => p.Invoke(pv.pokemonData));

                var endFilter = nameFilter && compoundLanguageFilter && compoundOwnershipFilter && compoundRarityFilter;
                pv.gameObject.SetActive(endFilter);
            });
            filterSurvivorsCount.text = allViews.Values.Count(pv => pv.gameObject.activeSelf) + " / " + MaxPokemon;
        }

        public void AddRarityFilter(Predicate<PokemonData> rarityFilter)
        {
            rarityFilters.Add(rarityFilter);
            ResetDisplayWithFilters();
        }
        
        public void RemoveRarityFilter(Predicate<PokemonData> rarityFilter)
        {
            rarityFilters.Remove(rarityFilter);
            ResetDisplayWithFilters();
        }

        public void AddLanguageFilter(Predicate<PokemonData> languageFilter)
        {
            languageFilters.Add(languageFilter);
            ResetDisplayWithFilters();
        }

        public void RemoveLanguageFilter(Predicate<PokemonData> languageFilter)
        {
            languageFilters.Remove(languageFilter);
            ResetDisplayWithFilters();
        }

        public void AddOwnershipFilter(Predicate<PokemonData> ownershipFilter)
        {
            ownershipFilters.Add(ownershipFilter);
            ResetDisplayWithFilters();
        }

        public void RemoveOwnershipFilter(Predicate<PokemonData> ownershipFilter)
        {
            ownershipFilters.Remove(ownershipFilter);
            ResetDisplayWithFilters();
        }

        public void SetNameSearch(string nameSearch)
        {
            this.nameSearch = nameSearch;
            ResetDisplayWithFilters();
        }

        public void ClearFilters()
        {
            rarityFilters.Clear();
            languageFilters.Clear();
            ownershipFilters.Clear();
            nameSearch = "";
            ResetDisplayWithFilters();
        }
        
        private void FillPokemonViews()
        {
            var allPokemonData = databaseManager.GetAllPokemonData();
            var allPokemonDict = new Dictionary<int, PokemonData>();
            allPokemonData.ForEach(data => allPokemonDict.Add(data.PokedexNb, data));

            for (var pokemonId = 1; pokemonId <= MaxPokemon; pokemonId++)
                InitializePokemonView(allPokemonDict.ContainsKey(pokemonId)
                    ? allPokemonDict[pokemonId]
                    : new PokemonData {PokedexNb = pokemonId, Owned = false});
        }
    }
}