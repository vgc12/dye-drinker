using UnityEngine;

namespace StateMachines.Shotgun
{
    public class ShotgunDescendedState : ShotgunBaseState
    {
        public ShotgunDescendedState(Mechanics.Shotgun shotgun, Animator animator) : base(shotgun, animator)
        {
            
        }
        
        public override async void Enter()
        {
            base.Enter();
            await Shotgun.WaitBeforeShooting();
        }
        
        public override void Update()
        {
            base.Update();
            Shotgun.TrackPlayer();
        }
        
        
        
    }
    
    public class ShotgunShootState : ShotgunBaseState
    {
        public ShotgunShootState(Mechanics.Shotgun shotgun, Animator animator) : base(shotgun, animator)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            Shotgun.Shoot();
        }
        
        public override void Update()
        {
            base.Update();
            Shotgun.TrackPlayer();
        }
        
        
        
    }
}