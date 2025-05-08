using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [SerializeField] GameLevelListSO _gameLevelList;

    public GameData GameData;
    
    private string _filePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _filePath = Application.persistentDataPath + "/" + "ProjectKitchenRobot.dat";
        if (File.Exists(_filePath))
        {
            GameData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
        }
        else
        {
            GameData = new GameData();

            GameData.SetLevelCompletion(0, GameLevelStatus.Unlocked);

            File.WriteAllText(_filePath, JsonUtility.ToJson(GameData));
        }
    }

    public GameLevelStatus IsLevelCompleted(GameLevelSO gameLevel)
    {

        int level = _gameLevelList.Levels.IndexOf(gameLevel);
        if (!GameData.CompletedLevels.ToDictionary().ContainsKey(level))
        {
            return GameLevelStatus.Locked;
        }
        return GameData.CompletedLevels.ToDictionary()[level];
    }

    public void SetLevelCompletion(GameLevelSO gameLevel)
    {
        int level = _gameLevelList.Levels.IndexOf(gameLevel);
        if (level == -1)
        {
            return;
        }
        GameData.SetLevelCompletion(level, GameLevelStatus.Completed);
        GameData.SetLevelCompletion(level + 1, GameLevelStatus.Unlocked);
        File.WriteAllText(_filePath, JsonUtility.ToJson(GameData));
    }
}

[System.Serializable]
public class GameData
{
    public SerializableDictionary<int, GameLevelStatus> CompletedLevels = new SerializableDictionary<int, GameLevelStatus>();

    public void SetLevelCompletion(int level, GameLevelStatus levelStatus)
    {
        CompletedLevels.Add(level, levelStatus);
    }

    public bool IsLevelCompleted(int level)
    {
        return CompletedLevels.TryGetValue(level, out GameLevelStatus levelStatus) && levelStatus == GameLevelStatus.Completed;
    }
}

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary[key] = value;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return dictionary;
    }
}

[System.Serializable]
public enum GameLevelStatus
{
    Completed,
    Unlocked,
    Locked
}