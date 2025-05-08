using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private GameLevelSO _gameLevel;
    public GameLevelSO GameLevel { get { return _gameLevel; } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(GameLevelSO level)
    {
        Debug.Log("LoadLevelCalled");
        _gameLevel = level;
        
        Invoke(nameof(LoadGameScene), 0.1f);
    }

    private void LoadGameScene()
    {
        SceneHandler.Instance.LoadScene(GameSceneNames.GameScene.ToString());
    }
}
