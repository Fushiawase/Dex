using System.Collections.Generic;
using System.IO;
using Domain;
using SQLite4Unity3d;
using UnityEngine;

namespace InfoLoader
{
    public class DatabaseManager : MonoBehaviour
    {
        private SQLiteConnection db;

        private void Awake()
        {
            var dbPath = Path.Combine(Application.persistentDataPath, "game.db");
            db = new SQLiteConnection(dbPath);
            db.CreateTable<PokemonData>();
            db.CreateTable<FileData>();
        }

        public void AddPokemonData(PokemonData pokemonData) => db.Insert(pokemonData);

        public void UpdatePokemonData(PokemonData pokemonData) => db.Update(pokemonData);

        public PokemonData GetPokemonData(int nb) => db.Find<PokemonData>(nb);
        
        public List<PokemonData> GetAllPokemonData() => db.Query<PokemonData>("SELECT * FROM PokemonData");


        public void AddFileData(FileData fileData) => db.Insert(fileData);

        public void DeleteFileData(FileData fileData) => db.Delete(fileData);

        public FileData GetFileData(string filePath) => db.Find<FileData>(filePath);
        
        public List<FileData> GetAllFileData() => db.Query<FileData>("SELECT * FROM FileData");
    }
}