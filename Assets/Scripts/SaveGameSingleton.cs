using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum ESerializationType
{
    JSON,
    Binary
}

[System.Serializable]
public class SaveData
{
    public int LivesLeft = 10;
    public bool IsFullScreen = true;
    public string LastUnlockedScene = "Level 1"; // Default to first level
}

public class SaveGameSingleton : MonoBehaviour
{
    private static SaveGameSingleton _instance;
    private static readonly object _lock = new object();

    public static SaveGameSingleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SaveGameSingleton>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("SaveGameManager");
                        _instance = obj.AddComponent<SaveGameSingleton>();
                        DontDestroyOnLoad(obj);
                        _instance.Initialize();
                    }
                }
                return _instance;
            }
        }
    }

    [SerializeField] private ESerializationType SerializationType = ESerializationType.JSON;
    private string SaveFilePath => $"{Application.persistentDataPath}/SaveGameData.json";
    private SaveData _saveGameData = new SaveData();

    public UnityEvent<SaveData> OnSaveRequestedEvent = new UnityEvent<SaveData>();
    public UnityEvent<SaveData> OnLoadRequestedEvent = new UnityEvent<SaveData>();

    private void Initialize()
    {
        // Removed SetupSerializationType() call as we handle serialization in the methods directly
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SaveGameToFile()
    {
        try
        {
            OnSaveRequestedEvent?.Invoke(_saveGameData);

            switch (SerializationType)
            {
                case ESerializationType.JSON:
                    SaveAsJson();
                    break;
                case ESerializationType.Binary:
                    SaveAsBinary();
                    break;
                default:
                    SaveAsJson(); // Default to JSON if unknown type
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public void LoadSaveGameFromFile()
    {
        try
        {
            switch (SerializationType)
            {
                case ESerializationType.JSON:
                    LoadFromJson();
                    break;
                case ESerializationType.Binary:
                    LoadFromBinary();
                    break;
                default:
                    LoadFromJson(); // Default to JSON if unknown type
                    break;
            }
            OnLoadRequestedEvent?.Invoke(_saveGameData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load save game: {e.Message}");
            _saveGameData = new SaveData(); // Reset to default
        }
    }

    private void SaveAsJson()
    {
        string json = JsonUtility.ToJson(_saveGameData, true);
        File.WriteAllText(SaveFilePath, json);
    }

    private void LoadFromJson()
    {
        if (!File.Exists(SaveFilePath)) return;
        string json = File.ReadAllText(SaveFilePath);
        _saveGameData = JsonUtility.FromJson<SaveData>(json);
    }

    private void SaveAsBinary()
    {
        Debug.LogWarning("Binary serialization is not recommended due to security concerns");
        // Using JSON as fallback for binary mode
        SaveAsJson();
    }

    private void LoadFromBinary()
    {
        Debug.LogWarning("Binary deserialization is not recommended due to security concerns");
        // Using JSON as fallback for binary mode
        LoadFromJson();
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        // Optional: Can be removed if not needed
        OnLoadRequestedEvent?.Invoke(_saveGameData);
    }

    public SaveData GetCurrentSaveData() => _saveGameData;

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}