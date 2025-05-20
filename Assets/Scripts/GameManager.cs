using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved game
            SaveGameSingleton.Instance.OnLoadRequestedEvent.AddListener((data) =>
            {
                string savedScene = data.LastUnlockedScene;

                if (!string.IsNullOrEmpty(savedScene))
                {
                    Debug.Log($"Loading saved scene: {savedScene}");
                    SceneManager.LoadScene(savedScene);
                }
                else
                {
                    Debug.LogWarning("Saved scene was empty. Loading default.");
                    SceneManager.LoadScene("Scene1"); // fallback
                }
            });

            SaveGameSingleton.Instance.LoadSaveGameFromFile();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No more levels.");
        }
    }

public void GameOver()
    {
        gameOverScreen.SetActive(true);
        //Time.timeScale = 0f;
    }
}
