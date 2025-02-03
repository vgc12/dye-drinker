using System;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuUI
    {
        public Action OnPlayButtonClicked {set => _buttonPlay.clicked += value;}
       // public Action OnDesktopButtonClicked {set => _buttonDesktop.clicked += value;}
        public Action OnCreditsButtonClicked {set => _buttonCredits.clicked += value;}
        public Action OnSettingsButtonClicked {set => _buttonSettings.clicked += value;}
        
        public Action OnControlsButtonClicked {set => _buttonControls.clicked += value;}
        
        private readonly Button _buttonPlay;
        private readonly Button _buttonDesktop;
        private readonly Button _buttonCredits;
        private readonly Button _buttonSettings;
        private readonly Button _buttonControls;

        public MainMenuUI(VisualElement root)
        {
            _buttonPlay = root.Q<Button>("ButtonPlay");
           // _buttonDesktop = root.Q<Button>("ButtonDesktop");
            _buttonCredits = root.Q<Button>("ButtonCredits");
            _buttonControls = root.Q<Button>("ButtonControls");
            _buttonSettings = root.Q<Button>("ButtonSettings");
        }
    }
}