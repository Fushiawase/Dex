using System;
using Domain;
using InfoLoader;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class DataEditScreen : MonoBehaviour
    {
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private PokemonDisplay pokemonDisplay;

        [SerializeField] private CustomButton owned;
        [SerializeField] private CustomButton nonOwned;

        [SerializeField] private GameObject raritySection;
        
        [SerializeField] private CustomButton common;
        [SerializeField] private CustomButton holo;
        [SerializeField] private CustomButton ex;
        [SerializeField] private CustomButton ir;

        [SerializeField] private GameObject languageSection;

        [SerializeField] private CustomButton french;
        [SerializeField] private CustomButton english;
        [SerializeField] private CustomButton japanese;
        [SerializeField] private CustomButton korean;

        [SerializeField] private Button cancel;
        [SerializeField] private Button apply;

        [SerializeField] private PokemonView displayedView;

        private PokemonView currentEditingView;

        private void Awake()
        {
            owned.DefineBehaviour(ClickOwned);
            nonOwned.DefineBehaviour(ClickNonOwned);
            common.DefineBehaviour(ClickCommon);
            holo.DefineBehaviour(ClickHolo);
            ex.DefineBehaviour(ClickEx);
            ir.DefineBehaviour(ClickIr);
            french.DefineBehaviour(ClickFrench);
            english.DefineBehaviour(ClickEnglish);
            japanese.DefineBehaviour(ClickJapanese);
            korean.DefineBehaviour(ClickKorean);
            
            cancel.onClick.AddListener(() => gameObject.SetActive(false));
            apply.onClick.AddListener(() =>
            {
                currentEditingView.pokemonData = displayedView.pokemonData.Clone();

                var dataDb = databaseManager.GetPokemonData(currentEditingView.pokemonData.PokedexNb);
                if(dataDb != null) 
                    databaseManager.UpdatePokemonData(currentEditingView.pokemonData);
                else
                    databaseManager.AddPokemonData(currentEditingView.pokemonData);
                
                currentEditingView.UpdateView();
                pokemonDisplay.ResetDisplayWithFilters();
                gameObject.SetActive(false);
            });
        }

        public void Initialize(PokemonView currentEditingView)
        {
            gameObject.SetActive(true);
            this.currentEditingView = currentEditingView;

            displayedView.pokemonData = currentEditingView.pokemonData.Clone();
            displayedView.UpdateView();
            displayedView.artwork.sprite = currentEditingView.artwork.sprite;
            displayedView.nameFr.text = displayedView.pokemonData.NameFr;
            displayedView.SetDexNb(displayedView.pokemonData.PokedexNb);

            switch (displayedView.pokemonData.Owned)
            {
                case true:
                    ClickOwned();
                    break;
                case false:
                    ClickNonOwned();
                    break;
            }

            switch (displayedView.pokemonData.Rarity)
            {
                case CardRarity.Common:
                    ClickCommon();
                    break;
                case CardRarity.Holo:
                    ClickHolo();
                    break;
                case CardRarity.Ex:
                    ClickEx();
                    break;
                case CardRarity.Ir:
                    ClickIr();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (displayedView.pokemonData.OwnedLanguage)
            {
                case Language.FRENCH:
                    ClickFrench();
                    break;
                case Language.ENGLISH:
                    ClickEnglish();
                    break;
                case Language.JAPANESE:
                    ClickJapanese();
                    break;
                case Language.KOREAN:
                    ClickKorean();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ClickOwned()
        {
            UnSelectButtons(nonOwned);
            owned.Select(true);
            displayedView.pokemonData.Owned = true;
            displayedView.UpdateView();
            raritySection.SetActive(true);
            languageSection.SetActive(true);
        }

        private void ClickNonOwned()
        {
            UnSelectButtons(owned);
            nonOwned.Select(true);
            displayedView.pokemonData.Owned = false;
            displayedView.UpdateView();
            raritySection.SetActive(false);
            languageSection.SetActive(false);
        }

        private void ClickCommon()
        {
            UnSelectButtons(holo, ex, ir);
            common.Select(true);
            displayedView.pokemonData.Rarity = CardRarity.Common;
            displayedView.UpdateView();
        }

        private void ClickHolo()
        {
            UnSelectButtons(common, ex, ir);
            holo.Select(true);
            displayedView.pokemonData.Rarity = CardRarity.Holo;
            displayedView.UpdateView();
        }

        private void ClickEx()
        {
            UnSelectButtons(common, holo, ir);
            ex.Select(true);
            displayedView.pokemonData.Rarity = CardRarity.Ex;
            displayedView.UpdateView();
        }

        private void ClickIr()
        {
            UnSelectButtons(common, holo, ex);
            ir.Select(true);
            displayedView.pokemonData.Rarity = CardRarity.Ir;
            displayedView.UpdateView();
        }

        private void ClickFrench()
        {
            UnSelectButtons(english, japanese, korean);
            french.Select(true);
            displayedView.pokemonData.OwnedLanguage = Language.FRENCH;
            displayedView.UpdateView();
        }

        private void ClickEnglish()
        {
            UnSelectButtons(french, japanese, korean);
            english.Select(true);
            displayedView.pokemonData.OwnedLanguage = Language.ENGLISH;
            displayedView.UpdateView();
        }

        private void ClickJapanese()
        {
            UnSelectButtons(french, english, korean);
            japanese.Select(true);
            displayedView.pokemonData.OwnedLanguage = Language.JAPANESE;
            displayedView.UpdateView();
        }

        private void ClickKorean()
        {
            UnSelectButtons(french, english, japanese);
            korean.Select(true);
            displayedView.pokemonData.OwnedLanguage = Language.KOREAN;
            displayedView.UpdateView();
        }

        private void UnSelectButtons(params CustomButton[] buttons)
        {
            foreach (var customButton in buttons) customButton.Select(false);
        }
    }
}