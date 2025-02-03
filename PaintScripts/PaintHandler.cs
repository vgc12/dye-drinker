
using System;
using System.Linq;
using EventBus;
using EventChannels;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace PaintScripts
{
    public class PaintHandler : MonoBehaviour
    {
        public static PaintHandler Instance { get; private set; }
      
        public int CansLeft {get; private set;}
  
        public bool deathTimerEnabled = true;
        [SerializeField] private float amountToAddToTimer = 2f;
        public float deathTimer = 15f;
        [SerializeField]
        private AudioSource winSound;
        
        public Volume volume;
        private float _minimum;
        private float _maximum = 1.0f;
        private float _t;
        public int TotalCans {get; private set;}
        private EventBinding<PaintConsumedEvent> _paintConsumedEventBinding;
        private EventBinding<PlayerCaughtEvent> _playerCaughtEventBinding;
        [SerializeField] private AudioSource deathScreamVoiceLine;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                volume.enabled = true;
               
                _paintConsumedEventBinding = new EventBinding<PaintConsumedEvent>(OnPaintConsumed);
                _playerCaughtEventBinding = new EventBinding<PlayerCaughtEvent>(OnPlayerCaught);
                EventBus<PaintConsumedEvent>.Register(_paintConsumedEventBinding);
                EventBus<PlayerCaughtEvent>.Register(_playerCaughtEventBinding);
               
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            var cans = FindObjectsByType<PaintCan>(FindObjectsSortMode.None);
            int length = cans.Length;
            
            foreach (var paintCan in cans)
            {
                paintCan.enabled = true;
            }
                
            switch (GameManager.Instance.Difficulty)
            {
                case Difficulty.Easy:
                    amountToAddToTimer = 3f;
                    cans = cans.OrderBy(x => Random.Range(0, length)).Take(72).ToArray();
                    DisableCans();
                    break;
                case Difficulty.Medium:
                    amountToAddToTimer = 2.625f;
                    cans = cans.OrderBy(x => Random.Range(0, length)).Take(30).ToArray();
                    DisableCans();
                    break;
                case Difficulty.Hard:
                    amountToAddToTimer = 2.25f;
                    cans = cans.OrderBy(x => Random.Range(0, length)).Take(0).ToArray();
                    DisableCans();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
          
               
            

            CansLeft = length - cans.Length;
            TotalCans = CansLeft;
            deathTimerEnabled = true;

            void DisableCans()
            {
                foreach (var can in cans)
                {
                    can.gameObject.SetActive(false);
                }
            }
        }
        private void Update()
        {
            CountDownDeathTimer();

            ChangeVisualEffects();
        }
        

        private void OnPlayerCaught(PlayerCaughtEvent arg0)
        {
            deathTimerEnabled = false;
        }

        private void OnPaintConsumed(PaintConsumedEvent arg0)
        {
          
            
            CansLeft--;

            if (deathTimerEnabled)
            {
                deathTimer += amountToAddToTimer;
            }

            if (CansLeft > 0) return;
            winSound.Play();
            GameManager.Instance.ChangeGameState(GameState.Win);

        }


        private void CountDownDeathTimer()
        {
            if (deathTimerEnabled && GameManager.Instance.gameState == GameState.Playing)
            {
                deathTimer -= Time.deltaTime;
      
            }

            if (!(deathTimer <= 0) || GameManager.Instance.gameState != GameState.Playing) return;
            deathScreamVoiceLine.Play();
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }

        private void ChangeVisualEffects()
        {
            _t += (TotalCans-CansLeft) * 0.01f * Time.deltaTime;

            if(volume.profile.TryGet(out ChromaticAberration chromaticAberration))
            {
                chromaticAberration.intensity.value = Mathf.Lerp(_minimum, _maximum, _t);
            }
            if(volume.profile.TryGet(out PaniniProjection paniniProjection))
            {
                paniniProjection.distance.value = Mathf.Lerp(_minimum, _maximum, _t);
            }
        
            
                
            if (!(_t > 1.0f)) return;
           
            (_maximum, _minimum) = (_minimum, _maximum);
            _t = 0.0f;
        }

        private void OnDisable()
        {
            EventBus<PaintConsumedEvent>.Deregister(_paintConsumedEventBinding);
            EventBus<PlayerCaughtEvent>.Deregister(_playerCaughtEventBinding);
        }
    }
}