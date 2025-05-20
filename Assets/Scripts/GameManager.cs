using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    public static GameManager instance;

    private const string DEFAULT_SCENE = "MainMenu";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSaveSystem()
    {
        if (SaveGameSingleton.Instance == null)
        {
            Debug.LogError("SaveGameSingleton not initialized!");
            SceneManager.LoadScene(DEFAULT_SCENE);
            return;
        }

        SaveGameSingleton.Instance.OnLoadRequestedEvent.AddListener(LoadSavedScene);
        SaveGameSingleton.Instance.LoadSaveGameFromFile();
    }

    private void LoadSavedScene(SaveData data)
    {
        if (data == null || string.IsNullOrEmpty(data.LastUnlockedScene))
        {
            Debug.LogWarning("No saved scene found, loading default");
            SceneManager.LoadScene(DEFAULT_SCENE);
            return;
        }

        try
        {
            Debug.Log($"Loading saved scene: {data.LastUnlockedScene}");
            SceneManager.LoadScene(data.LastUnlockedScene);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load scene {data.LastUnlockedScene}: {e.Message}");
            SceneManager.LoadScene(DEFAULT_SCENE);
        }
    }

    public void NextLevel()
    {
        try
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.Log("No more levels available!");
                // Optionally load a completion scene or menu
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading next level: {e.Message}");
        }
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (SaveGameSingleton.Instance != null)
        {
            SaveGameSingleton.Instance.OnLoadRequestedEvent.RemoveListener(LoadSavedScene);
        }
    }
}