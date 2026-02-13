using System.IO;
using Domain;
using InfoLoader;
using TMPro;
using UnityEngine;

namespace App
{
    public class FileDataView : MonoBehaviour
    {
        [SerializeField] private CustomButton copy;
        [SerializeField] private CustomButton delete;
        [SerializeField] private TMP_Text pathText;

        private FileData fileData;
        
        public void Initialize(FileData fileData)
        {
            this.fileData = fileData;
            pathText.text = fileData.Path;
            delete.DefineBehaviour(OnDeletePress);
            
            copy.DefineBehaviour(() =>
            {
                if (!File.Exists(fileData.Path))
                {
                    OnDeletePress();
                    return;
                }
                
                CopyToClipboard(File.ReadAllText(fileData.Path));
            });
        }

        private void OnDeletePress()
        {
            if (File.Exists(fileData.Path)) File.Delete(fileData.Path);
            FindFirstObjectByType<DatabaseManager>().DeleteFileData(fileData);
            Destroy(gameObject);
        }
        
        private static void CopyToClipboard(string text)
        {
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            
            AndroidJavaObject clipboardManager = activity.Call<AndroidJavaObject>("getSystemService", "clipboard");
            
            AndroidJavaClass clipDataClass = new AndroidJavaClass("android.content.ClipData");
            AndroidJavaObject clip = clipDataClass.CallStatic<AndroidJavaObject>("newPlainText", "label", text);
            
            clipboardManager.Call("setPrimaryClip", clip);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur lors de la copie dans le presse-papier : " + e.Message);
        }
#endif
        }
    }
}