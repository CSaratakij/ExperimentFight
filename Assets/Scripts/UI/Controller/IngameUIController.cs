
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ExperimentFight
{
    public class IngameUIController : MonoBehaviour
    {
        enum IngameMenu
        {
            PauseMenu,
            HealthMenu
        }

        [SerializeField]
        RectTransform[] menus;

        [SerializeField]
        Button btnResume;


        void Awake()
        {
            Initialize();
        }

        void Update()
        {
            InputHandler();
        }

        void Initialize()
        {
            ShowPanel(IngameMenu.PauseMenu, false);
            ShowPanel(IngameMenu.HealthMenu, true);
        }

        void InputHandler()
        {
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

            GameController.gameState = isShow ? GameState.Pause : GameState.Normal;
        }

        void ShowPanel(IngameMenu menuType, bool value)
        {
            RectTransform menu = menus[(int) menuType];

            if (menu.gameObject.activeSelf == value)
                return;

            menu.gameObject.SetActive(value);

            Time.timeScale = (!value) ? 1.0f : 0.0f;
            GameController.gameState = (!value) ? GameState.Normal : GameState.Pause;
        }

        public void Hide()
        {
            ShowPanel(IngameMenu.PauseMenu, false);
        }
    }
}

