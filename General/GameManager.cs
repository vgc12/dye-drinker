using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using EventChannels;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool _cutscenePlayed = false;
    
  
    
    public GameState gameState;
    
    public Difficulty Difficulty { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Difficulty = Difficulty.Easy;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Game" when !_cutscenePlayed:
                ChangeGameState(GameState.Cutscene);
                _cutscenePlayed = true;
                break;
            case "Game" when _cutscenePlayed:
                ChangeGameState(GameState.Playing);
                break;
            case "MainMenu":
                _cutscenePlayed = false;
                break;
        }
    }
    
    public void ChangeGameState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.Win:
                Win();
                break;
            case GameState.Lose:
                Lose();
                break;
            case GameState.Playing:
                ResumeGame();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.Loading:
                break;
            case GameState.MainMenu:
                break;
            case GameState.Cutscene:
                break;
            
            
        }
        EventBus<GameStateChangedEvent>.Raise(new GameStateChangedEvent(state));
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        AudioListener.pause = true;
        Time.timeScale = 0;
    }
    
    

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    private void Lose()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
    }

    private void Win()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

internal struct GameStateChangedEvent : IEvent
{
    public readonly GameState GameState;

    public GameStateChangedEvent(GameState gameState)
    {
        GameState = gameState;
    }
}