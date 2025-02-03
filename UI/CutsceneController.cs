using EventBus;
using EventChannels;
using Player;
using UnityEngine;

namespace UI
{
    public class CutsceneController : MonoBehaviour
    {
        private Animator _animator;
        private PlayerLooking _playerLooking;
        private EventBinding<GameStateChangedEvent> _gameStateChangedEventBinding;
        [SerializeField] private AudioSource openingVoiceLine;
    
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerLooking = GetComponent<PlayerLooking>();
            _animator.enabled = false;
            _gameStateChangedEventBinding = new EventBinding<GameStateChangedEvent>(OnGameStateChanged);
            EventBus<GameStateChangedEvent>.Register(_gameStateChangedEventBinding);
        }

        private void OnGameStateChanged(GameStateChangedEvent state)
        {
            if (state.GameState == GameState.Cutscene)
            {
                _animator.enabled = true;
                _playerLooking.allowMovement = false;
                _animator.Play("Cutscene");
            
            }
            else
            {
                _animator.enabled = false;
                _playerLooking.allowMovement = true;
            }
        }

        public void PlayOpeningVoiceLine()
        {
            openingVoiceLine.Play();
        }
    
        public void DisableCustscene()
        {
            GameManager.Instance.ChangeGameState(GameState.Playing);
        }

        private void OnDisable()
        {
           EventBus<GameStateChangedEvent>.Deregister(_gameStateChangedEventBinding);
        }
    }
}
