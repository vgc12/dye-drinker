using EventBus;
using EventChannels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PaintScripts
{
    [DisallowMultipleComponent]
    public class PaintCan : MonoBehaviour
    {
    
        private EventBinding<InteractEvent> _interactEventBinding;
        
        [SerializeField] private Material paintMaterial;
        public Color paintColor;
        private void Awake()
        {
      
            _interactEventBinding = new EventBinding<InteractEvent>(OnInteract);
            EventBus<InteractEvent>.Register(_interactEventBinding);
            paintMaterial = GetComponent<Renderer>().materials[1];
            paintColor = Random.ColorHSV();
            paintMaterial.color = paintColor;
        }

        private void OnInteract(InteractEvent interaction)
        {
            if (interaction.Hit.transform != transform) return;
        
            gameObject.SetActive(false);
         
            EventBus<PaintConsumedEvent>.Raise(new PaintConsumedEvent(this));
        }

        private void OnDisable()
        {
            _interactEventBinding.OnEventRaised -= OnInteract;
        }
    }
}
