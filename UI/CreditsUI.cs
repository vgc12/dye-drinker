using System;
using UnityEngine.UIElements;

namespace UI
{
    public class CreditsUI
    {
        public Action ButtonMainMenuClicked { set => _buttonMainMenu.clicked += value; }
  
        private readonly Button _buttonMainMenu;
        
        public CreditsUI(VisualElement root)
        {
            _buttonMainMenu = root.Q<Button>("ButtonMainMenu");
        }
        
    }
}