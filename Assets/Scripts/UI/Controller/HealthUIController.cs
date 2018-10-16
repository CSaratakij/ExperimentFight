
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExperimentFight
{
    public class HealthUIController : MonoBehaviour
    {
        const string textHealthFormat = "Health : {0}";

        [SerializeField]
        Text txtHealth;

        [SerializeField]
        StockHealth stockHealth;


        void OnEnable()
        {
            UpdateStockHealthUI();
        }

        void Awake()
        {
            SubscribeEvent();
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void UpdateStockHealthUI()
        {
            UpdateStockHealthUI(stockHealth.Current);
        }

        void UpdateStockHealthUI(int value)
        {
            txtHealth.text = string.Format(textHealthFormat, value);
        }

        void SubscribeEvent()
        {
            stockHealth.OnStockHealthValueChanged += OnStockHealthValueChanged;
        }

        void UnsubscribeEvent()
        {
            stockHealth.OnStockHealthValueChanged -= OnStockHealthValueChanged;
        }

        void OnStockHealthValueChanged(int value)
        {
            UpdateStockHealthUI(value);
        }
    }
}

