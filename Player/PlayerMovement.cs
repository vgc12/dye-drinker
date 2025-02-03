

using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Shared")]
        private PlayerInputActions _playerInputActions;
        private Rigidbody _rb;

        [Header("Movement")]
        [SerializeField] 
        private float speed = 5f;
        [SerializeField]
        private float speedMultiplier = 10f;
        [SerializeField]
        private Transform orientation;
        private Vector3 _moveDirection;

        [Header("Slope Detection")]
        [SerializeField]
        private float maxSlopeAngle = 35f;
        private RaycastHit _slopeHit;


        [Header("Jumping")]
        [SerializeField]
        [Range(0.0f, 2f)]
        private float heightMultiplier = 1.5f;
        [SerializeField] 
        private LayerMask groundMask;
        [SerializeField]
        private float airMultiplier = 0.4f;

        [SerializeField]
        private float fallMultiplier = 2.5f;

    
        public bool IsGrounded { get; private set; }
        private float _playerheight = 1f;

        [Header("Drag")]
        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 2f;






        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;

            _playerInputActions = new PlayerInputActions();
      
            _playerInputActions.Player.Enable();
            //_playerInputActions.Player.Jump.performed += Jump;
       

            _playerheight = transform.localScale.y;
            IsGrounded = true;

            speed *= speedMultiplier;

        }

        private void Update()
        {
       
        }


        private void FixedUpdate() => MovePlayer();

        private void MovePlayer()
        {
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, _playerheight *heightMultiplier + .3f, groundMask);

  

            ControlSpeed();
            if(_rb.linearVelocity.y < 0)
            {
                _rb.linearVelocity += ((fallMultiplier - 1) * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up).normalized;
            
            }

            var direction = _playerInputActions.Player.Move.ReadValue<Vector2>();
            _moveDirection = (orientation.forward * direction.y) + (orientation.right * direction.x);


            if (OnSlope())
            {
                _rb.AddForce(speed * GetSlopeMovementDirection(), ForceMode.Force);
            }

            _rb.AddForce((IsGrounded ? 1 : airMultiplier) * speed * _moveDirection.normalized, ForceMode.Force);
        }



        private void ControlSpeed()
        {
            _rb.linearDamping = IsGrounded ? groundDrag : airDrag;

            Vector3 flatVelocity = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            if(flatVelocity.magnitude > speed)
            {
                flatVelocity = flatVelocity.normalized * speed;
                _rb.linearVelocity = new Vector3(flatVelocity.x, _rb.linearVelocity.y, flatVelocity.z);
            }
        }

        private bool OnSlope()
        {
            if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerheight * heightMultiplier + .3f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
           
                return angle < maxSlopeAngle && angle != 0f;
            }

            return false;
        }

        private Vector3 GetSlopeMovementDirection() => Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;

        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
        }
    }
}