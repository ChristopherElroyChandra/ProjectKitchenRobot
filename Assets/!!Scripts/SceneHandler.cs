using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    public event Action<String> OnSceneChange; //Called when about to change scene
    public event Action<Scene> OnSceneLoaded;   //Called when a new scene is loaded

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke(scene);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        OnSceneChange?.Invoke(sceneName);
    }
    public void QuitApp()
    {
        Application.Quit();
    }

}

public enum GameSceneNames
{
    MainMenuScene,
    GameScene
}