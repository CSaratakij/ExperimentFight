
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class StockHealth : MonoBehaviour
    {
        [SerializeField]
        int current;

        [SerializeField]
        int maximum;


        public bool IsEmpty { get { return current <= 0; } }
        public int Current { get { return current; } }
        public int Maximum { get { return maximum; } }


        public delegate void _Func(int value);
        public event _Func OnStockHealthValueChanged;


        void OnDestroy()
        {
            OnStockHealthValueChanged = null;
        }

        public void Clear()
        {
            current = 0;
            _FireEvent_OnStockHealthValueChanged();
        }

        public void ResetMaximum()
        {
            maximum = 0;
        }

        public void FullRestore()
        {
            current = maximum;
            _FireEvent_OnStockHealthValueChanged();
        }

        public void Restore(int value)
        {
            current = (current + value) > maximum ? maximum : (current + value);
            _FireEvent_OnStockHealthValueChanged();
        }

        public void Remove(int value)
        {
            current = (current - value) < 0 ? 0 : (current - value);
            _FireEvent_OnStockHealthValueChanged();
        }

        public void SetMaximum(int value)
        {
            maximum = value;
        }

        public void AddMaximum(int value)
        {
            maximum += value;
        }

        public void RemoveMaximum(int value)
        {
            maximum = (maximum - value) < 0 ? 0 : (maximum - value);
        }

        void _FireEvent_OnStockHealthValueChanged()
        {
            if (OnStockHealthValueChanged != null)
                OnStockHealthValueChanged(current);
        }
    }
}
