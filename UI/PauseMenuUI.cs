using System;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenuUI
    {
        public Action ButtonSettingsClicked { set => _buttonSettings.clicked += value; }
        public Action ButtonResumeClicked { set => _buttonResume.clicked += value; }
        public Action ButtonMenuClicked { set => _buttonMenu.clicked += value; }
 
        public Action ButtonRestartClicked { set => _buttonRestart.clicked += value; }
    
    
    
        private readonly Button _buttonSettings;
        private readonly Button _buttonResume;
        private readonly Button _buttonRestart;
        private readonly Button _buttonMenu;
        private readonly Button _buttonDesktop;
    
    
        public PauseMenuUI(VisualElement root)
        {
            _buttonResume = root.Q<Button>("ButtonResume");
            _buttonRestart = root.Q<Button>("ButtonRestart");
            _buttonMenu = root.Q<Button>("ButtonMenu");
            _buttonDesktop = root.Q<Button>("ButtonDesktop");
            _buttonSettings = root.Q<Button>("ButtonSettings");
       
        }
    }
}