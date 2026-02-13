using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain;
using InfoLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class FileSaveScreen : MonoBehaviour
    {
        [SerializeField] private Button back;
        [SerializeField] private Button save;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private FileDataView fileDataViewPrefab;
        [SerializeField] private Transform content;
        

        private DatabaseManager databaseManager;
        
        private List<PokemonView> pokemonViews;

        private void Awake()
        {
            databaseManager = FindFirstObjectByType<DatabaseManager>();
            back.onClick.AddListener(() => gameObject.SetActive(false));
            save.onClick.AddListener(SaveFile);
        }

        private void Start()
        {
            var allDataFile = databaseManager.GetAllFileData();
            allDataFile.ForEach(fd => Instantiate(fileDataViewPrefab, content).Initialize(fd));
        }

        public void Initialize(List<PokemonView> pokemonViews)
        {
            gameObject.SetActive(true);
            this.pokemonViews = pokemonViews;
        }

        private void SaveFile()
        {
            var inputFieldText = inputField.text;
            if (string.IsNullOrWhiteSpace(inputFieldText))
                inputFieldText = "Dex_File";
            inputFieldText = Regex.Replace(inputFieldText, @"\.[^.]+$", "");
            inputFieldText = inputFieldText.Replace(" ", "_");
            inputFieldText += DateTime.Now.ToString("dd-MM-yyyy");
            inputFieldText += ".txt";

            var filePath = Path.Combine("/storage/emulated/0/Documents/", inputFieldText);
            
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true));
            foreach (var line in pokemonViews.Where(pv => pv.gameObject.activeSelf).Select(pv => pv.dexNb.text + " " + pv.nameFr.text))
                writer.WriteLine(line);
            
            if(databaseManager.GetFileData(filePath) != null)
                return;
            
            var fileData = new FileData {Path = filePath};
            databaseManager.AddFileData(fileData);
            Instantiate(fileDataViewPrefab, content).Initialize(fileData);
        }
    }
}