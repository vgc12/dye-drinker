using UnityEngine;

namespace StateMachines.Shotgun
{
    public abstract class ShotgunBaseState : BaseState
    {
        public Animator Animator {get; private set;}

        public Mechanics.Shotgun Shotgun {get; private set;}
        protected ShotgunBaseState(Mechanics.Shotgun shotgun, Animator animator)
        {
            Shotgun = shotgun;
            Animator = animator;
        }
    }
}