using System;
using Domain;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App
{
    public class PokemonView : MonoBehaviour
    {
        [SerializeField] public Image artwork;
        [SerializeField] public TMP_Text dexNb;
        [SerializeField] public TMP_Text nameFr;
        
        [SerializeField] private Button button;
        
        [SerializeField] private Image background;
        [SerializeField] private Image flag;
        [SerializeField] private Image rarity;

        //flags
        [SerializeField] private Sprite flagFr;
        [SerializeField] private Sprite flagEn;
        [SerializeField] private Sprite flagJp;
        [SerializeField] private Sprite flagKr;
        
        //Rarity
        [SerializeField] private Sprite normal;
        [SerializeField] private Sprite holo;
        [SerializeField] private Sprite ex;
        [SerializeField] private Sprite ir;
        
        public PokemonData pokemonData;

        private Color ownedYellow = new Color(1f, 0.8f, 0f);

        public void Initialize(PokemonData pokemonData, UnityAction onClick)
        {
            this.pokemonData = pokemonData;
            button.onClick.AddListener(onClick);
            UpdateView();
        }

        public void UpdateView()
        {
            if (pokemonData.Owned)
            {
                SetFlag(pokemonData.OwnedLanguage);
                SetRarity(pokemonData.Rarity);
            }
            
            flag.gameObject.SetActive(pokemonData.Owned);
            rarity.gameObject.SetActive(pokemonData.Owned);
            background.color = pokemonData.Owned ? ownedYellow : Color.white;
        }

        private Sprite LanguageEnumToSprite(Language language) => language switch
        {
            Language.FRENCH => flagFr,
            Language.ENGLISH => flagEn,
            Language.JAPANESE => flagJp,
            Language.KOREAN => flagKr,
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

        private Sprite RarityEnumToSprite(CardRarity cardRarity) => cardRarity switch
        {
            CardRarity.Common => normal,
            CardRarity.Holo => holo,
            CardRarity.Ex => ex,
            CardRarity.Ir => ir,
            _ => throw new ArgumentOutOfRangeException(nameof(cardRarity), cardRarity, null)
        };

        private void SetFlag(Language language) => flag.sprite = LanguageEnumToSprite(language);

        private void SetRarity(CardRarity cardRarity) => rarity.sprite = RarityEnumToSprite(cardRarity);
        
        public void SetDexNb(int nb) => dexNb.text = "#" + new string('0',  5 - ("#" + nb).Length) + nb;
    }
}