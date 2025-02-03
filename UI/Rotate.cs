using UnityEngine;

namespace UI
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }
    public class Rotate : MonoBehaviour
    {
    
        public RotationAxis rotationAxis;
        private void Update()
        {
            Vector3 rotation = transform.localEulerAngles;
            if (rotationAxis == RotationAxis.X)
                rotation.x += Time.deltaTime * 20;
            else if (rotationAxis == RotationAxis.Y)
                rotation.y += Time.deltaTime * 20;
            else
                rotation.z += Time.deltaTime * 20;
            transform.localEulerAngles = rotation;
        }
    }
}