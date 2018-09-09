
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (Input.GetButtonDown("Cancel"))
                TogglePanel(IngameMenu.PauseMenu);
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
            Time.timeScale = (!value) ? 1.0f : 0.0f;
        }
    }
}

