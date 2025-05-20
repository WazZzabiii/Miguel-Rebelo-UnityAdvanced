using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            string nextSceneName = GetNextSceneName();

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                // Save the next scene name
                SaveGameSingleton.Instance.OnSaveRequestedEvent.AddListener((data) => {
                    data.LastUnlockedScene = nextSceneName;
                });

                SaveGameSingleton.Instance.SaveGameToFile();
            }

            // Load the next level
            GameManager.instance.NextLevel();
        }
    }

    private string GetNextSceneName()
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
}
