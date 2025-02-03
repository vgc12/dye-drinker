using UnityEngine;

namespace Mechanics
{
    public class SecurityCamera : MonoBehaviour
    {
        public Transform caughtZone;
        void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            transform.LookAt(caughtZone.position);
        }
    }
}
