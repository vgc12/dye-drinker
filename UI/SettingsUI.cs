using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UI
{
    public class SettingsUI 
    {
    

        public Action ButtonBackClicked { set => _buttonBack.clicked += value; }
        public Action ButtonSaveClicked { set => _buttonSave.clicked += value; }
    
    
       // private readonly SliderInt _sliderResolution;
        private readonly Slider _sliderMouseSensitivity;
        private readonly Slider _sliderMasterVolume;
        private readonly Slider _sliderSoundEffectsVolume;
        private readonly Slider _sliderDialogVolume;
    
        private readonly Button _buttonBack;
        private readonly Button _buttonSave;

        private readonly List<string> _resolutions = new List<string>()
        {
            "640x480",
            "1280x720",
            "1920x1080",
            "2560x1440",
            "3840x2160"
        };
    
        public SettingsUI(VisualElement root, SaveManager saveManager)
        {
         //   _sliderResolution = root.Q<SliderInt>("SliderResolution");
            _sliderDialogVolume = root.Q<Slider>("SliderVolumeDialog");
            _sliderMasterVolume = root.Q<Slider>("SliderVolumeMaster");
            _sliderSoundEffectsVolume = root.Q<Slider>("SliderVolumeEffects");
            _sliderMouseSensitivity = root.Q<Slider>("SliderSensitivity");
        
            var labelResolution = root.Q<Label>("LabelResolution");
        
            //_sliderResolution.RegisterValueChangedCallback((evt) => labelResolution.text = _resolutions[evt.newValue]);
        
        
            _buttonBack = root.Q<Button>("ButtonBack");
            _buttonSave = root.Q<Button>("ButtonSave");
        
       
            _buttonSave.clicked += SaveSettings;
        
            SetUIValues(saveManager.CurrentSettings);
        }
    
    
    
    

        private void SaveSettings()
        {
            /*
            var resolution = GetResolution(_sliderResolution.value);
        
            var s = new Settings(_sliderResolution.value, resolution[0], resolution[1], _sliderMouseSensitivity.value, _sliderMasterVolume.value, _sliderSoundEffectsVolume.value, _sliderDialogVolume.value);
        */
            var s = new Settings( _sliderMouseSensitivity.value, _sliderMasterVolume.value, _sliderSoundEffectsVolume.value, _sliderDialogVolume.value);
            
            SaveManager.Instance.SaveSettings(s);
        }

        /*
        private int[] GetResolution(int index)
        {
            var resolution = _resolutions[index].Split('x');
            var x = int.Parse(resolution[0]);
            var y = int.Parse(resolution[1]);
            return new[] {x, y};
        }
        */

        private void SetUIValues(Settings settings)
        {
          //  _sliderResolution.value = settings.ResolutionIndex;
            _sliderMouseSensitivity.value = settings.MouseSensitivity;
            _sliderMasterVolume.value = settings.MasterVolume;
            _sliderSoundEffectsVolume.value = settings.SoundEffectsVolume;
            _sliderDialogVolume.value = settings.DialogVolume;
        
        }
    
    
    
    }
}