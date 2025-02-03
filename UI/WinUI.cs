using System;
using UnityEngine.UIElements;

namespace UI
{
    public class WinUI
    {
        public Action ButtonMenuClicked { set => _buttonMenu.clicked += value; }
        public Action ButtonDesktopClicked { set => _buttonDesktop.clicked += value; }
            
        private Button _buttonMenu;
        private Button _buttonDesktop;
        
        public WinUI(VisualElement root)
        {
            _buttonMenu = root.Q<Button>("ButtonMenu");
            _buttonDesktop = root.Q<Button>("ButtonDesktop");
        }
        
    }
}