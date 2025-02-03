using UnityEngine;

namespace StateMachines.Shotgun
{
    public class ShotgunDescendingState : ShotgunBaseState
    {
        public ShotgunDescendingState(Mechanics.Shotgun shotgun, Animator animator) : base(shotgun, animator)
        {
            
        }
        
        public override async void Enter()
        {
            base.Enter();
            
            Animator.CrossFade("Descend", 0.1f);
            Shotgun.StartDescendSoundEffect();
            await Shotgun.Descend();
        }
        
        
        
    }
}

