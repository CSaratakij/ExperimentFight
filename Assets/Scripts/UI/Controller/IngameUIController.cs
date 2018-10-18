using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace ExperimentFight
{
    public class IngameUIController : MonoBehaviour
    {
        enum IngameMenu
        {
            PauseMenu,
            HealthMenu,
            MainMenu
        }

        [SerializeField]
        RectTransform[] menus;

        [SerializeField]
        Button btnResume;

        [SerializeField]
        Button btnStart;

        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void Start()
        {
            EventSystem eventSystem = EventSystemInstance.instance.eventSystem;
            EventSystemInstance.instance.eventSystem.SetSelectedGameObject(btnStart.gameObject, new BaseEventData(eventSystem));
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void Update()
        {
            InputHandler();
        }

        void Initialize()
        {
            ShowPanel(IngameMenu.HealthMenu, false);
            ShowPanel(IngameMenu.MainMenu, true);
        }

        void SubscribeEvents()
        {
            GameController.OnGameStart += OnGameStart;
            GameController.OnGameOver += OnGameOver;
        }

        void UnsubscribeEvents()
        {
            GameController.OnGameStart -= OnGameStart;
            GameController.OnGameOver -= OnGameOver;
        }

        void OnGameStart()
        {
            ShowPanel(IngameMenu.MainMenu, false);
            ShowPanel(IngameMenu.HealthMenu, true);
        }

        void OnGameOver()
        {
            ShowPanel(IngameMenu.PauseMenu, true);
        }

        void InputHandler()
        {
            if (menus[(int)IngameMenu.MainMenu].gameObject.activeSelf)
                return;

            bool isToggleShow = false;

            if (GameController.gameState == GameState.Pause)
                isToggleShow = Input.GetButtonDown("Cancel") || Input.GetButtonDown("PauseMenu");

            else
                isToggleShow = Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("PauseMenu");

            if (isToggleShow)
            {
                TogglePanel(IngameMenu.PauseMenu);
                EventSystem eventSystem = EventSystemInstance.instance.eventSystem;
                EventSystemInstance.instance.eventSystem.SetSelectedGameObject(btnResume.gameObject, new BaseEventData(eventSystem));
            }
        }

        void TogglePanel(IngameMenu menuType)
        {
            RectTransform menu = menus[(int) menuType];
            bool isShow = menu.gameObject.activeSelf;

            isShow = !isShow;
            ShowPanel(menuType, isShow);
        }

        void ShowPanel(IngameMenu menuType, bool value)
        {
            RectTransform menu = menus[(int) menuType];

            if (menu.gameObject.activeSelf == value)
                return;

            menu.gameObject.SetActive(value);

            if (menuType == IngameMenu.PauseMenu)
            {
                Time.timeScale = (!value) ? 1.0f : 0.0f;
                GameController.gameState = (!value) ? GameState.Normal : GameState.Pause;
            }
            else
            {
                Time.timeScale = 1.0f;
                GameController.gameState = GameState.Normal;
            }
        }

        public void Hide()
        {
            ShowPanel(IngameMenu.PauseMenu, false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GameStart()
        {
            GameController.GameStart();
        }

        public void GameStop()
        {
            GameController.GameStop();
        }

        public void Restart()
        {
            GameController.GameStop();
            SceneManager.LoadScene(0);
        }
    }
}

