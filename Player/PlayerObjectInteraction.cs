using EventBus;
using EventChannels;
using Mechanics;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerObjectInteraction : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private LayerMask interactionLayerMask;
        [SerializeField] private PlayerInputActions _playerInputActions;
        [SerializeField] private SlurpHandler slurpHandler;
        
    
        [SerializeField] private Transform playerLooking;
    
        private readonly RaycastHit[] _hits = new RaycastHit[1];
        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Interact.performed += Interact;
            _playerInputActions.Player.Interact.Enable();
        
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerLooking.transform.position, playerLooking.transform.forward * interactionDistance);
        }

        private void Interact(InputAction.CallbackContext obj)
        {
            if (Physics.RaycastNonAlloc(playerLooking.transform.position, playerLooking.transform.forward, _hits,
                    interactionDistance, interactionLayerMask) <= 0 || !slurpHandler.canSlurp) return;
        
        
            EventBus<InteractEvent>.Raise(new InteractEvent(_hits[0], obj));;
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Interact.performed -= Interact;
            _playerInputActions.Player.Interact.Disable();
        
        }
    }
}
