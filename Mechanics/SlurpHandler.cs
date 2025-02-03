using System.Collections;
using System.Linq;
using EventBus;
using EventChannels;
using UnityEngine;

namespace Mechanics
{
    public class SlurpHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource slurpSound;
        private EventBinding<PaintConsumedEvent> _paintConsumedEventBinding;
        private EventBinding<PlayerCaughtEvent> _playerCaughtEventBinding;
        private Renderer _renderer;
        private Animator _animator;
        public bool canSlurp = true;

        [SerializeField] private AudioSource caughtVoiceLine;
    
        [SerializeField] private AudioSource[] voiceLines;
   
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<Renderer>();
            _paintConsumedEventBinding = new EventBinding<PaintConsumedEvent>(OnPaintConsumed);
            _playerCaughtEventBinding = new EventBinding<PlayerCaughtEvent>(OnPlayerCaught);
            EventBus<PaintConsumedEvent>.Register(_paintConsumedEventBinding);
            EventBus<PlayerCaughtEvent>.Register(_playerCaughtEventBinding);
        }

        private void OnPlayerCaught(PlayerCaughtEvent arg0)
        {
            canSlurp = false;
        
            StartCoroutine(PlayCaughtVoiceLine());
        }

        private IEnumerator PlayCaughtVoiceLine()
        {
            voiceLines.ToList().ForEach(voiceLine => voiceLine.Stop());
            yield return new WaitForSeconds(1);
            caughtVoiceLine.Play();
            yield return null;
        }

        private void OnPaintConsumed(PaintConsumedEvent arg0)
        {
            _renderer.materials[1].color = arg0.PaintCan.paintColor;
            _animator.Play("Drink");
        }
    
        public void StartSlurpSoundEffect()
        {
            slurpSound.Play();
        }
    
        public void ToggleSurpability()
        {
            canSlurp = !canSlurp;
        }
    
        public void PlayVoiceLine()
        {
            if(UnityEngine.Random.Range(0, 100) < 15 )
                voiceLines[UnityEngine.Random.Range(0, voiceLines.Length)].Play();
        }
    
        private void OnDisable()
        {
            EventBus<PaintConsumedEvent>.Deregister(_paintConsumedEventBinding);
            EventBus<PlayerCaughtEvent>.Deregister(_playerCaughtEventBinding);
            StopCoroutine(PlayCaughtVoiceLine());
        }
    }
}
