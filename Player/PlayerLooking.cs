using EventBus;
using EventChannels;
using UnityEngine;

namespace Player
{
    public class PlayerLooking : MonoBehaviour
    {
    
        private EventBinding<SettingsChangedEvent> _settingsChangedEventBinding;
        private PlayerInputActions _playerInputActions;

        [SerializeField] 
        private Transform orientation;


        public float sensX = 100f;
        public float sensY = 100f;

        private float _xRotation;
        private float _yRotation;
    

        public bool allowMovement = true;


        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
            _settingsChangedEventBinding = new EventBinding<SettingsChangedEvent>(OnSettingsChanged);
            EventBus<SettingsChangedEvent>.Register(_settingsChangedEventBinding);
        
        }

        private void OnSettingsChanged(SettingsChangedEvent settingsChangedEvent)
        {
            SetSensitivity(settingsChangedEvent.Settings.MouseSensitivity);
        }


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    
        public void SetSensitivity(float sensitivity)
        {
            sensX = sensitivity;
            sensY = sensitivity;
        }

        // Update is called once per frame
        private void Update()
        {
            if (allowMovement)
                MoveCamera();


        }

        private void MoveCamera()
        {
       
            Vector2 mouseDelta = _playerInputActions.Player.Look.ReadValue<Vector2>();

            float mouseX = mouseDelta.x * Time.deltaTime * sensX;
            float mouseY = mouseDelta.y * Time.deltaTime * sensY;

            _yRotation += mouseX;
            _xRotation -= mouseY;

            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }

        private void OnDisable()
        {
            EventBus<SettingsChangedEvent>.Deregister(_settingsChangedEventBinding);
            _playerInputActions.Player.Disable();
        }
    }
}