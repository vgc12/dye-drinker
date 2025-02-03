using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    




    public async void LoadLevel(string levelName)
    {
        

        GameManager.Instance.ChangeGameState(GameState.Loading);
        AsyncOperation scene =  SceneManager.LoadSceneAsync(levelName);

        if (scene == null) return;
        scene.allowSceneActivation = true;

        while (scene.progress < .9f)
        {
            await Awaitable.WaitForSecondsAsync(1f);
        }
    }
}
