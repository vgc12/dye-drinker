using EventBus;
using EventChannels;
using PaintScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
    
    
        private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset playingScreenAsset;

        private VisualElement _root;
        private VisualElement _rootVisualElement;
        private VisualElement _playingUI;
        private VisualElement _pauseUI;
        private VisualElement _deathUI;
        private VisualElement _loadingUI;
        private VisualElement _cutsceneUI;
        private VisualElement _winUI;
        private VisualElement _currentUI;
        private VisualElement _settingsUI;
        private Label _labelCansLeft;
        private Label _labelWidthdrawalNumber;
    
         private EventBinding<GameStateChangedEvent> _gameStateChangedEventBinding;
        private EventBinding<PaintConsumedEvent> _paintConsumedEventBinding;
        [SerializeField] private GameObject uiPaintCan;
        [SerializeField] private Volume volume;
    
        private Label _labelWithdrawlText;
    
        private PlayerInputActions _playerInputActions;


        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.Pause.Enable();
            _playerInputActions.UI.Pause.performed += PauseGame;
            _gameStateChangedEventBinding = new EventBinding<GameStateChangedEvent>(OnGameStateChanged);
            _paintConsumedEventBinding = new EventBinding<PaintConsumedEvent>(OnPaintConsumed);
            EventBus<GameStateChangedEvent>.Register(_gameStateChangedEventBinding);
            EventBus<PaintConsumedEvent>.Register(_paintConsumedEventBinding);
    
        }
    

        private void OnEnable()
        {
          
            _uiDocument = GetComponent<UIDocument>();
            _uiDocument.visualTreeAsset = playingScreenAsset;
            _root = _uiDocument.rootVisualElement;
        
            InitializeUIScreens();
        
            SetupPauseUI();
            SetupDeathUI();
            SetupWinUI();
      
        
        
            _labelCansLeft.style.color = Color.black;
            uiPaintCan.GetComponent<Renderer>().materials[1].color = Color.black;
        
        }

        private void Start()
        {
            SetupSettingsUI();
            _labelCansLeft.text = PaintHandler.Instance.CansLeft + "/" + PaintHandler.Instance.TotalCans;
        }


        private void InitializeUIScreens()
        {
            _playingUI = _root.Q<VisualElement>("PlayingUI");
            _pauseUI = _root.Q<VisualElement>("PauseUI");
            _deathUI = _root.Q<VisualElement>("DeathUI");
            _loadingUI = _root.Q<VisualElement>("LoadingUI");
            _cutsceneUI = _root.Q<VisualElement>("CutsceneUI");
            if(GameManager.Instance.Difficulty == Difficulty.Hard)
                _winUI = _root.Q<VisualElement>("HardmodeWinUI");
            else
                _winUI = _root.Q<VisualElement>("WinUI");
            
          
            _settingsUI = _root.Q<VisualElement>("SettingsUI");
            _currentUI = _playingUI;
            _labelCansLeft = _root.Q<Label>("LabelCansLeft");
        }

        private void OnDisable()
        {
            _playerInputActions.UI.Pause.performed -= PauseGame;
            EventBus<GameStateChangedEvent>.Deregister(_gameStateChangedEventBinding);
            EventBus<PaintConsumedEvent>.Deregister(_paintConsumedEventBinding);
            _playerInputActions.Disable();
        }

        private void PauseGame(InputAction.CallbackContext obj)
        {
            switch (GameManager.Instance.gameState)
            {
                case GameState.Playing:
                    GameManager.Instance.ChangeGameState(GameState.Paused);
                    break;
                case GameState.Paused:
                    GameManager.Instance.ChangeGameState(GameState.Playing);
                    break;
            }
        }

 


        private void OnPaintConsumed(PaintConsumedEvent paintConsumed)
        {
            if (GameManager.Instance.gameState != GameState.Playing) return;
            _labelCansLeft.text = PaintHandler.Instance.CansLeft + "/" + PaintHandler.Instance.TotalCans;
            _labelCansLeft.style.color = paintConsumed.PaintCan.paintColor;
            uiPaintCan.GetComponent<Renderer>().materials[1].color = paintConsumed.PaintCan.paintColor;
        }


        private void Update()
        {
        
            if (GameManager.Instance.gameState != GameState.Playing) return;
            _labelWidthdrawalNumber = _root.Q<Label>("LabelWithdrawalNumber");
            _labelWidthdrawalNumber.text = Mathf.Round(PaintHandler.Instance.deathTimer) + " SECONDS";

            _labelWidthdrawalNumber.style.color = PaintHandler.Instance.deathTimer switch
            {
                > 15f => Color.green,
                > 10f => Color.yellow,
                _ => Color.red
            };
        }

        private void OnGameStateChanged(GameStateChangedEvent gameState)
        {

            uiPaintCan.gameObject.SetActive(false);
            ToggleDepthOfField(false);
            switch (gameState.GameState)
            {
                case GameState.Lose:
                {
                    UIExtensions.ChangeMenu(_deathUI,ref _currentUI);
                
                    break;
                }
                case GameState.Playing:
                {
                    UIExtensions.ChangeMenu(_playingUI, ref _currentUI);
            
                
           
                    uiPaintCan.gameObject.SetActive(true);
                    break;
                }
                case GameState.Paused:
                {
                    ToggleDepthOfField(true);
                    UIExtensions.ChangeMenu(_pauseUI, ref _currentUI);
               

                    break;
                }
            
                case GameState.Loading:
                {
                    UIExtensions.ChangeMenu(_loadingUI, ref _currentUI);
                
                    break;
                }
                case GameState.Cutscene:
                {
                    uiPaintCan.gameObject.SetActive(false);
                    UIExtensions.ChangeMenu(_cutsceneUI, ref _currentUI);
                    break;
                }
                case GameState.Win:
                {
                    UIExtensions.ChangeMenu(_winUI, ref _currentUI);
                
                    break;
                }
            }

     
        }
    
        private void SetupSettingsUI()
        {
            var settingsUI = new SettingsUI(_settingsUI, SaveManager.Instance)
            {
                ButtonBackClicked = () => UIExtensions.ChangeMenu(_pauseUI, ref _currentUI),
                ButtonSaveClicked = () => UIExtensions.ChangeMenu(_pauseUI, ref _currentUI),
            };
        }
    
        private void SetupDeathUI()
        {
            var deathScreenUI = new DeathScreenUI(_deathUI, PaintHandler.Instance)
            {
                OnRestartButtonClicked = () => LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().name),
                OnMenuButtonClicked = () => LevelManager.Instance.LoadLevel("MainMenu")
            };
            
        }

        private void SetupWinUI()
        {
            var winScreenUI = new WinUI(_winUI)
            {
                ButtonMenuClicked = () => LevelManager.Instance.LoadLevel("MainMenu"),
                ButtonDesktopClicked = () => GameManager.Instance.QuitGame()
            };
            
        
        }
    
        private void SetupPauseUI()
        {
            var pauseMenuUI = new PauseMenuUI(_pauseUI)
            {
                ButtonResumeClicked = () => GameManager.Instance.ChangeGameState(GameState.Playing),
                ButtonRestartClicked = () => LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().name),
                ButtonMenuClicked = () =>
                {
                    Time.timeScale = 1;
                    AudioListener.pause = false;
                    LevelManager.Instance.LoadLevel("MainMenu");
                },
                ButtonSettingsClicked = () => UIExtensions.ChangeMenu(_settingsUI, ref _currentUI),
               // ButtonDesktopClicked = () => GameManager.Instance.QuitGame()
            };
        }

   
        private void ToggleDepthOfField(bool active)
        {
            if(volume.profile.TryGet(out DepthOfField depthOfField))
            {
                depthOfField.mode.value = active ? DepthOfFieldMode.Bokeh : DepthOfFieldMode.Off;
            
            }
        } 
    }
}
