using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private GenericEventChannelScriptableObject<SettingsChangedEvent> settingsChangedEventChannel;
    private EventBinding<SettingsChangedEvent> _settingsChangedEventBinding;
    [SerializeField] private AudioMixer audioMixer;
    
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            _settingsChangedEventBinding = new EventBinding<SettingsChangedEvent>(OnSettingsChanged);
            EventBus<SettingsChangedEvent>.Register(_settingsChangedEventBinding);
        
           
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EventBus<SettingsChangedEvent>.Deregister(_settingsChangedEventBinding);
    }

    private void OnSettingsChanged(SettingsChangedEvent settingsChangedEvent)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(settingsChangedEvent.Settings.MasterVolume) * 20);
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10( settingsChangedEvent.Settings.SoundEffectsVolume) * 20);
        audioMixer.SetFloat("VoiceLinesVolume", Mathf.Log10(settingsChangedEvent.Settings.DialogVolume) * 20);
    }
}
