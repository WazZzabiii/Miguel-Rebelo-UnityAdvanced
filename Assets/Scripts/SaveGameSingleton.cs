using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum ESerializationType
{
    JSON,
    Binary,
    XML
}

[System.Serializable]
public class SaveData
{
    public int LivesLeft = 10;
    public bool IsFullScreen = true;

    // 🔥 Add this to store scene progress
    public string LastUnlockedScene = "Scene1"; // default fallback scene
}

public class SaveGameSingleton
{
    private static SaveGameSingleton instance;

    public static SaveGameSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveGameSingleton();
                instance.SetupCallbacks();
                instance.SetupSerializationType();
            }

            return instance;
        }
    }

#if UNITY_STANDALONE
    private ESerializationType SerializationType = ESerializationType.Binary;
#else
    private ESerializationType SerializationType = ESerializationType.JSON;
#endif

    private string saveToFilePath = $"{Application.persistentDataPath}/SaveGameData.json";

    private SaveData SaveGameData = new SaveData();

    public UnityEvent<SaveData> OnSaveRequestedEvent = new UnityEvent<SaveData>();
    public UnityEvent<SaveData> OnLoadRequestedEvent = new UnityEvent<SaveData>();

    private Action<SaveData> SerializationAction;
    private Action<SaveData> DeserializationAction;

    public void SaveGameToFile()
    {
        OnSaveRequestedEvent?.Invoke(SaveGameData);
        SerializationAction?.Invoke(SaveGameData);
    }

    public void LoadSaveGameFromFile()
    {
        DeserializationAction?.Invoke(SaveGameData);
        OnLoadRequestedEvent?.Invoke(SaveGameData);
    }

    private void SetupCallbacks()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        OnLoadRequestedEvent?.Invoke(SaveGameData);
    }

    private void SetupSerializationType()
    {
        switch (SerializationType)
        {
            case ESerializationType.Binary:
                SerializationAction += BinarySerializationAction;
                DeserializationAction += BinaryDeserializationAction;
                break;
            case ESerializationType.JSON:
                SerializationAction += JSONSerializationAction;
                DeserializationAction += JSONDeserializationAction;
                break;
            case ESerializationType.XML:
            default:
                Debug.LogError("Unsupported Serialization Type");
                break;
        }
    }

    private void JSONSerializationAction(SaveData obj)
    {
        string json = JsonUtility.ToJson(SaveGameData);
        File.WriteAllText(saveToFilePath, json);
    }

    private void JSONDeserializationAction(SaveData obj)
    {
        if (!File.Exists(saveToFilePath)) return;

        string json = File.ReadAllText(saveToFilePath);
        SaveGameData = JsonUtility.FromJson<SaveData>(json);
    }

    private void BinarySerializationAction(SaveData obj)
    {
        using (FileStream stream = File.Open(saveToFilePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, SaveGameData);
        }
    }

    private void BinaryDeserializationAction(SaveData obj)
    {
        if (!File.Exists(saveToFilePath)) return;

        using (FileStream stream = File.Open(saveToFilePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveGameData = (SaveData)formatter.Deserialize(stream);
        }
    }

    public SaveData GetCurrentSaveData()
    {
        return SaveGameData;
    }
}

