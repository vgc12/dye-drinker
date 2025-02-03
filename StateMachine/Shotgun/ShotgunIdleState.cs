using StateMachines.Shotgun;
using UnityEngine;

namespace StateMachines
{
    public class ShotgunIdleState : ShotgunBaseState
    {
        public ShotgunIdleState(Mechanics.Shotgun shotgun, Animator animator) : base(shotgun, animator)
        {
        }
    }
}