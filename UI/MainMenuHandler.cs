using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI
{
    public class MainMenuHandler : MonoBehaviour
    {

    
        private VisualElement _root;
        private VisualElement _warningUI;
        private VisualElement _mainMenuUI;
        private VisualElement _creditsUI;
        private VisualElement _loadingUI;
        private VisualElement _settingsUI;
        private VisualElement _currentUI;
        private VisualElement _difficultyUI;
        private VisualElement _controlsUI;
    
        private UIDocument _uiDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
        }

        private void OnEnable()
        {
            _warningUI = _root.Q<VisualElement>("WarningUI");
            _mainMenuUI = _root.Q<VisualElement>("MainMenuUI");
            
            _creditsUI = _root.Q<VisualElement>("CreditsUI");
            _loadingUI = _root.Q<VisualElement>("LoadingUI");
            _settingsUI = _root.Q<VisualElement>("SettingsUI");
            _difficultyUI = _root.Q<VisualElement>("DifficultyUI");
            _controlsUI = _root.Q<VisualElement>("ControlsUI");
            _currentUI = _warningUI;

        
            SetUpCreditsUI();
            SetUpMainMenuUI();
            SetupSettingsUI();
            SetUpDifficultyUI();
            SetUpControlsUI();
        }

 
        private void SetUpDifficultyUI()
        {
            var difficultyUI = new DifficultyUI(_root)
            {
                EasyButtonClicked = () => StartGame(Difficulty.Easy),
                MediumButtonClicked = () => StartGame(Difficulty.Medium),
                HardButtonClicked = () => StartGame(Difficulty.Hard),
                BackButtonClicked = () => UIExtensions.ChangeMenu(_mainMenuUI, ref _currentUI)
            };
        }

        private void SetUpCreditsUI()
        {
            var creditsUI = new CreditsUI(_root)
            {
                ButtonMainMenuClicked = () => UIExtensions.ChangeMenu(_mainMenuUI, ref _currentUI)
            };
        }

        private void SetUpMainMenuUI()
        {
            var menuUI = new MainMenuUI(_root)
            {
                OnPlayButtonClicked = () => UIExtensions.ChangeMenu(_difficultyUI, ref _currentUI),
                OnCreditsButtonClicked = OnCreditsButtonClicked,
                OnControlsButtonClicked = () => UIExtensions.ChangeMenu(_controlsUI, ref _currentUI),
               // OnDesktopButtonClicked = OnQuitButtonClicked,
                OnSettingsButtonClicked = () => UIExtensions.ChangeMenu(_settingsUI, ref _currentUI)
            };
        }
        
        private void SetUpControlsUI()
        {
            var controlsUI = new ControlsUI(_root)
            {
                OnBackButtonClicked = () => UIExtensions.ChangeMenu(_mainMenuUI, ref _currentUI)
            };
        }

        private IEnumerator Start()
        {
            UIExtensions.ChangeMenu(_warningUI, ref _currentUI);
        
            yield return new WaitForSeconds(2);
            UIExtensions.ChangeMenu(_mainMenuUI, ref _currentUI);
            yield return new WaitForSeconds(2);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        private void OnCreditsButtonClicked()
        {
            UIExtensions.ChangeMenu(_creditsUI, ref _currentUI);
        
        }





        private  void StartGame(Difficulty difficulty)
        {
            GameManager.Instance.Difficulty = difficulty;
            UIExtensions.ChangeMenu(_loadingUI, ref _currentUI);
            LevelManager.Instance.LoadLevel("Game");
        }

        private void SetupSettingsUI()
        {
            var settingsUI = new SettingsUI(_settingsUI, SaveManager.Instance)
            {
                ButtonBackClicked = () => UIExtensions.ChangeMenu(_mainMenuUI, ref _currentUI),
                ButtonSaveClicked = () => UIExtensions.ChangeMenu(_mainMenuUI, ref _currentUI),
            };
        }
    
    
 
    }
}
