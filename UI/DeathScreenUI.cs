using System;
using EventBus;
using EventChannels;
using PaintScripts;
using UnityEngine.UIElements;

namespace UI
{
    public class DeathScreenUI
    {
 
        
        public Action OnRestartButtonClicked {set => _buttonRestart.clicked += value;}
        public Action OnMenuButtonClicked {set => _buttonMenu.clicked += value;}
      
        
    
        
        private readonly Button _buttonRestart;
        private readonly Button _buttonMenu;

        private readonly PaintHandler _paintHandler;
        private readonly VisualElement _root;
        readonly EventBinding<GameStateChangedEvent> _gameStateChangedEventBinding;
        
        public DeathScreenUI(VisualElement root, PaintHandler paintHandler)
        {
          
            _gameStateChangedEventBinding = new EventBinding<GameStateChangedEvent>(OnGameStateChanged);
            EventBus<GameStateChangedEvent>.Register(_gameStateChangedEventBinding);
         _paintHandler = paintHandler;
            _root = root;
            _buttonRestart = root.Q<Button>("ButtonRestart");
            _buttonMenu = root.Q<Button>("ButtonMenu");
        
        
            
            
        }

        private void OnGameStateChanged(GameStateChangedEvent obj)
        {
            if (obj.GameState != GameState.Lose) return;
            var labelDeathReason = _root.Q<Label>("LabelDeathReason");
            labelDeathReason.text = _paintHandler.deathTimer <= 0 ? "Death by paint withdrawal" : "Should have been sneakier...";
        }
        
        ~DeathScreenUI()
        {
            EventBus<GameStateChangedEvent>.Deregister(_gameStateChangedEventBinding);
        }
    }
}