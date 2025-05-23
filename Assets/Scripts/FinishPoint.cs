using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            string nextSceneName = GetNextSceneName();

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                // Safely handle save game operations
                if (SaveGameSingleton.Instance != null)
                {
                    SaveGameSingleton.Instance.GetCurrentSaveData().LastUnlockedScene = nextSceneName;
                    SaveGameSingleton.Instance.SaveGameToFile();
                }
                else
                {
                    Debug.LogWarning("SaveGameSingleton instance not found!");
                }
            }

            // Safely load next level
            if (GameManager.instance != null)
            {
                GameManager.instance.NextLevel();
            }
            else
            {
                Debug.LogError("GameManager instance not found!");
                //SceneManager.LoadScene(nextSceneName); // Fallback
            }
        }
    }

    private string GetNextSceneName()
    {
        try
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(nextIndex);
                return Path.GetFileNameWithoutExtension(path);
            }
            return ""; // No next scene
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error getting next scene name: {e.Message}");
            return "";
        }
    }
}