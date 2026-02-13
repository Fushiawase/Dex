#if UNITY_EDITOR

using System.Collections;
using System.IO;
using App;
using Domain;
using SQLite4Unity3d;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagement
{
    public class PokemonViewSpawner : MonoBehaviour
    {
        private const int MaxPokemon = 1025;
        
        //private void Start() => StartCoroutine(RetrieveAllSprites());

        private IEnumerator RetrieveAllSprites()
        {
            for (var pokemonId = 1; pokemonId <= MaxPokemon; pokemonId++)
            {
                var spriteUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{pokemonId}.png";
                using var spriteRequest = UnityWebRequestTexture.GetTexture(spriteUrl);
                yield return spriteRequest.SendWebRequest();
                
                if (spriteRequest.result == UnityWebRequest.Result.Success)
                {
                    var sprite = ((DownloadHandlerTexture) spriteRequest.downloadHandler).texture.EncodeToPNG();

                    var tempFolder = Application.persistentDataPath + "/TempSprites";
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }
                    var tempFilePath = tempFolder + "/" + pokemonId + ".png";
                    File.WriteAllBytes(tempFilePath, sprite);
                }
                else
                    Debug.LogError($"Error while retrieving sprite of Pokémon nb {pokemonId} : {spriteRequest.error}");
            }
        }
        
        // Il faut d'abord mettre tous les sprites qui sont dans Application.persistentDataPath manuellement dans le projet
        [MenuItem("Tools/Instantiate Object")]
        public static void InstantiateObject()
        {
            var dbPath = Path.Combine(Application.persistentDataPath, "game.db");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<PokemonData>();
            
            var content = Object.FindFirstObjectByType<GridLayoutGroup>();
            var prefab = AssetDatabase.LoadAssetAtPath<PokemonView>("Assets/Prefabs/PokemonView.prefab");
            for (var i = 1; i <= 1025; i++)
            {
                var pokemonView = Instantiate(prefab, content.transform);
                var loadedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/" + i + ".png");
                Debug.Log(loadedSprite);
                pokemonView.artwork.sprite = loadedSprite;
                pokemonView.SetDexNb(i);
                var data = db.Find<PokemonData>(i);
                pokemonView.nameFr.text = data.NameFr;
            }
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
    }
}

#endif