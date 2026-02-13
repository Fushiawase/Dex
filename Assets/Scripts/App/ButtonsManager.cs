using System;
using System.Collections;
using System.Collections.Generic;
using Domain;
using TMPro;
using UnityEngine;

namespace App
{
    public class ButtonsManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameSearch;
        [SerializeField] private CustomButton spamSearch;
        [SerializeField] private CustomButton clearSearch;

        [SerializeField] private CustomButton owned;
        [SerializeField] private CustomButton nonOwned;
        [SerializeField] private CustomButton reset;

        [SerializeField] private CustomButton common;
        [SerializeField] private CustomButton holo;
        [SerializeField] private CustomButton ex;
        [SerializeField] private CustomButton ir;

        [SerializeField] private CustomButton french;
        [SerializeField] private CustomButton english;
        [SerializeField] private CustomButton japanese;
        [SerializeField] private CustomButton korean;

        private PokemonDisplay pokemonDisplay;
        private Coroutine coroutine;
        
        private void Awake()
        {
            pokemonDisplay = FindFirstObjectByType<PokemonDisplay>();

            nameSearch.onValueChanged.AddListener(s => pokemonDisplay.SetNameSearch(s));
            
            clearSearch.DefineBehaviour(() =>
            {
                pokemonDisplay.SetNameSearch("");
                nameSearch.text = "";
            });
            
            spamSearch.DefineBehaviour(() =>
            {
                IEnumerator DelayedNameSearchReset()
                {
                    yield return new WaitForSeconds(0.8f);
                    
                    pokemonDisplay.SetNameSearch("");
                    nameSearch.text = "";
                }

                void SpamSearchListener(string s)
                {
                    if(coroutine != null)
                        StopCoroutine(coroutine);

                    coroutine = StartCoroutine(DelayedNameSearchReset());
                }
                
                if (spamSearch.selected)
                {
                    nameSearch.onValueChanged.RemoveListener(SpamSearchListener);
                    spamSearch.Select(false);
                }
                else
                {
                    nameSearch.onValueChanged.AddListener(SpamSearchListener);
                    spamSearch.Select(true);
                }
            });

            reset.DefineBehaviour(() =>
            {
                pokemonDisplay.ClearFilters();
                nameSearch.text = "";
                new List<CustomButton>{owned, nonOwned, common, holo, ex, ir, french, english, japanese, korean}.ForEach(b => b.Select(false));
            });
            
            void ButtonBehaviour(CustomButton button, Predicate<PokemonData> predicate, Action<Predicate<PokemonData>> addFilter, Action<Predicate<PokemonData>> removeFilter)
            {
                if (button.selected)
                {
                    button.Select(false);
                    removeFilter(predicate);
                }
                else
                {
                    button.Select(true);
                    addFilter(predicate);
                }
            }
            
            owned.DefineBehaviour(() =>
            {
                bool OwnedFilter(PokemonData pokemonData) => pokemonData.Owned;

                ButtonBehaviour(owned, OwnedFilter, pokemonDisplay.AddOwnershipFilter, pokemonDisplay.RemoveOwnershipFilter);
            });
            
            nonOwned.DefineBehaviour(() =>
            {
                bool NonOwnedFilter(PokemonData pokemonData) => !pokemonData.Owned;

                ButtonBehaviour(nonOwned, NonOwnedFilter, pokemonDisplay.AddOwnershipFilter, pokemonDisplay.RemoveOwnershipFilter);
            });

            common.DefineBehaviour(() =>
            {
                bool CommonFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.Rarity == CardRarity.Common;

                ButtonBehaviour(common, CommonFilter, pokemonDisplay.AddRarityFilter, pokemonDisplay.RemoveRarityFilter);
            });
            
            holo.DefineBehaviour(() =>
            {
                bool HoloFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.Rarity == CardRarity.Holo;

                ButtonBehaviour(holo, HoloFilter, pokemonDisplay.AddRarityFilter, pokemonDisplay.RemoveRarityFilter);
            });
            
            ex.DefineBehaviour(() =>
            {
                bool ExFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.Rarity == CardRarity.Ex;

                ButtonBehaviour(ex, ExFilter, pokemonDisplay.AddRarityFilter, pokemonDisplay.RemoveRarityFilter);
            });
            
            ir.DefineBehaviour(() =>
            {
                bool IrFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.Rarity == CardRarity.Ir;

                ButtonBehaviour(ir, IrFilter, pokemonDisplay.AddRarityFilter, pokemonDisplay.RemoveRarityFilter);
            });
            
            french.DefineBehaviour(() =>
            {
                bool FrenchFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.OwnedLanguage == Language.FRENCH;

                ButtonBehaviour(french, FrenchFilter, pokemonDisplay.AddLanguageFilter, pokemonDisplay.RemoveLanguageFilter);
            });
            
            english.DefineBehaviour(() =>
            {
                bool EnglishFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.OwnedLanguage == Language.ENGLISH;

                ButtonBehaviour(english, EnglishFilter, pokemonDisplay.AddLanguageFilter, pokemonDisplay.RemoveLanguageFilter);
            });
            
            japanese.DefineBehaviour(() =>
            {
                bool JapaneseFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.OwnedLanguage == Language.JAPANESE;

                ButtonBehaviour(japanese, JapaneseFilter, pokemonDisplay.AddLanguageFilter, pokemonDisplay.RemoveLanguageFilter);
            });
            
            korean.DefineBehaviour(() =>
            {
                bool KoreanFilter(PokemonData pokemonData) => pokemonData.Owned && pokemonData.OwnedLanguage == Language.KOREAN;

                ButtonBehaviour(korean, KoreanFilter, pokemonDisplay.AddLanguageFilter, pokemonDisplay.RemoveLanguageFilter);
            });
        }
    }
}