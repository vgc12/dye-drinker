
using EventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EventChannels
{
    public struct InteractEvent : IEvent
    {
        public RaycastHit Hit;
        public InputAction.CallbackContext Context;
    
        public InteractEvent(RaycastHit hit, InputAction.CallbackContext context)
        {
            Hit = hit;
            Context = context;
        }
    }
}
