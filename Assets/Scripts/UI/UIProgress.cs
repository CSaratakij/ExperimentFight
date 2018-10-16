using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class UIProgress : MonoBehaviour
    {
        public static UIProgress instance = null;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        float progressValue;

        [SerializeField]
        Transform ui;

        [SerializeField]
        Transform uiProgress;

        void Awake()
        {
            MakeSingleton();
        }

        void Start()
        {
            Show(false);
            SetProgress(0);
        }

        void MakeSingleton()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void SetProgress(float value)
        {
            progressValue = value;
            Vector3 newScale = uiProgress.localScale;

            newScale.x = progressValue;
            uiProgress.localScale = newScale;
        }

        public void Show(bool value)
        {
            if (ui.gameObject.activeSelf != value)
                ui.gameObject.SetActive(value);
        }

        public void Show(bool value, Vector3 position)
        {
            transform.position = position;
            Show(value);
        }
    }
}
