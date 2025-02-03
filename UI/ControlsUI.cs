using System;
using UnityEngine.UIElements;

namespace UI
{
    public class ControlsUI
    {
        public Action OnBackButtonClicked {set => _buttonBack.clicked += value;}
        private readonly Button _buttonBack;
        
        public ControlsUI(VisualElement root)
        {
            _buttonBack = root.Q<Button>("Controls_ButtonMainMenu");
        
        
        }
    }
}