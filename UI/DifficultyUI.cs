using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace UI
{
    public class DifficultyUI
    {
        public Action EasyButtonClicked { set => _buttonEasy.clicked += value; }
        public Action MediumButtonClicked { set => _buttonMedium.clicked += value; }
        public Action HardButtonClicked { set => _buttonHard.clicked += value; }
        public Action BackButtonClicked { set => _buttonBack.clicked += value; }
    
        private readonly Button _buttonEasy;
        private readonly Button _buttonMedium;
        private readonly Button _buttonHard;
        private readonly Button _buttonBack;
    
        public DifficultyUI(VisualElement root)
        {
            _buttonEasy = root.Q<Button>("ButtonEasy");
            _buttonMedium = root.Q<Button>("ButtonMedium");
            _buttonHard = root.Q<Button>("ButtonHard");
            _buttonBack = root.Q<Button>("ButtonBackDifficulty");
            
        }
    }
}