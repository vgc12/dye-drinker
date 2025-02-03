using System;
using System.Collections;
using System.Threading.Tasks;
using EventBus;
using EventChannels;
using UnityEngine;
using StateMachines;
using StateMachines.Shotgun;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Mechanics
{
    public class Shotgun : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private EventBinding<PlayerCaughtEvent> _playerCaughtEventBinding;
        private Animator _animator;
        [SerializeField] private Transform shotgunObject;
        [SerializeField] private AudioSource descendSoundEffect;
        [SerializeField] private AudioSource shootSoundEffect;

        [SerializeField] private float timeBeforeShotFired = 3f;
        private float _descendWaitTime;
        private bool _playerCaught = false;
        
        private PlayerInputActions _inputActions;
        
        private StateMachine _stateMachine;

        private bool _restartHeld;
        
        private UnityAction<float> _onReloadHeld;
        
        private void Awake()
        {
            _animator = GetComponentInParent<Animator>();
            _restartHeld = false;
            _inputActions = new PlayerInputActions();
            _inputActions.Enable();
            _restartHeld = false;
            _inputActions.Player.Reload.performed += ReloadPressed;
            
            
            _descendWaitTime = descendSoundEffect.clip.length;
            _playerCaughtEventBinding = new EventBinding<PlayerCaughtEvent>(OnPlayerCaught);
            EventBus<PlayerCaughtEvent>.Register(_playerCaughtEventBinding);
            
            _stateMachine = new StateMachine();
            var idleState = new ShotgunIdleState(this, _animator);
            var descendState = new ShotgunDescendingState(this, _animator);
            var descendedState = new ShotgunDescendedState(this, _animator);
            var shootState = new ShotgunShootState(this, _animator);
            
            _stateMachine.AddTransition(idleState, descendState, new FuncPredicate(() => _playerCaught));
            _stateMachine.AddTransition(descendState, descendedState, new FuncPredicate(() => _descendWaitTime <=0));
            _stateMachine.AddTransition(descendedState, shootState, new FuncPredicate(() => timeBeforeShotFired <= 0));
            _stateMachine.AddAnyTransition(shootState, new FuncPredicate(() => _restartHeld));
            
            _stateMachine.SetState(idleState);
        }

        private void ReloadPressed(InputAction.CallbackContext obj)
        {
            _restartHeld = true;
        }


        private void OnPlayerCaught(PlayerCaughtEvent caughtEvent)
        {
            _playerCaught = true;
        }

        private void Update()
        {
            _stateMachine.Update();
        }
        
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }


        public void StartDescendSoundEffect()
        {
            descendSoundEffect.Play();
        }
    
        public void StopDescendSoundEffect()
        {
            descendSoundEffect.Stop();
        }

        public void Shoot()
        {
            _animator.CrossFade("Shoot", 0.1f);
            shootSoundEffect.Play();
            StartCoroutine(Lose()); 
        }

        private IEnumerator Lose()
        {
            yield return new WaitForSeconds(.1f);
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }


        
        public async Task Descend()
        {
           
            
            
            while (_descendWaitTime > 0)
            {
                _descendWaitTime -= Time.deltaTime;

                await Awaitable.EndOfFrameAsync();
            }

         
           

        }

        public async Task WaitBeforeShooting()
        {
            
            while (timeBeforeShotFired > 0)
            {
                timeBeforeShotFired -= Time.deltaTime;
                await Awaitable.EndOfFrameAsync();
            }
          
        }
        
        

        public void TrackPlayer() => shotgunObject.LookAt(player);

        private void OnDisable()
        {
        
            EventBus<PlayerCaughtEvent>.Deregister(_playerCaughtEventBinding);
        }
    }
}
