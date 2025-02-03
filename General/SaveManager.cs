using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EventBus;
using EventChannels;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public  class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private bool settingsLoaded = false;
    
    [SerializeField] private AudioMixer audioMixer;
    public Settings CurrentSettings { get; private set; } = new Settings(2, 1920, 1080, 50, 1, 1, 1);
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
           
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        CurrentSettings= LoadSettings();
        ApplySettings();
    }
    public void ApplySettings()
    {
        EventBus<SettingsChangedEvent>.Raise(new SettingsChangedEvent(CurrentSettings));
       // Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, true);
    }

    public void SaveSettings(Settings settings)
    {
        /*
        string json = JsonUtility.ToJson(settings);
        using var streamWriter = new StreamWriter(Application.persistentDataPath + "/settings.pnt");
        streamWriter.Write(json);
        Screen.SetResolution(settings.ResolutionX, settings.ResolutionY, true);
        */
        CurrentSettings = settings;
        EventBus<SettingsChangedEvent>.Raise(new SettingsChangedEvent(CurrentSettings));
    }

    private Settings LoadSettings()
    {
        /*
        if (File.Exists(Application.persistentDataPath + "/settings.pnt"))
        {
            using var streamReader = new StreamReader(Application.persistentDataPath + "/settings.pnt");
            string json = streamReader.ReadToEnd();
            return JsonUtility.FromJson<Settings>(json);
        }
        */
        if(settingsLoaded)
        {
            return CurrentSettings;
        }
        settingsLoaded = true;
        return new Settings( 50, 1, 1, 1);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public struct SettingsChangedEvent : IEvent
{
    public Settings Settings;
    public SettingsChangedEvent(Settings settings)
    {
        Settings = settings;
    }
    
}
