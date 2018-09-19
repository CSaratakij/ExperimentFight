
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

        public bool IsFull { get { return current == maximum; } }
        public bool IsEmpty { get { return current <= 0; } }

        public int Current { get { return current; } }
        public int Maximum { get { return maximum; } }


        public delegate void _Func();
        public delegate void _FuncValue(int value);

        public event _FuncValue OnStockHealthValueChanged;
        public event _Func OnStockHealthRestored;
        public event _Func OnStockHealthRemoved;


        void OnDestroy()
        {
            OnStockHealthValueChanged = null;
            OnStockHealthRestored = null;
            OnStockHealthRemoved = null;
        }

        public void Clear()
        {
            current = 0;
            _FireEvent_OnStockHealthValueChanged();
            _FireEvent_OnStockHealthRemoved();
        }

        public void ResetMaximum()
        {
            maximum = 0;
        }

        public void FullRestore()
        {
            current = maximum;
            _FireEvent_OnStockHealthValueChanged();
            _FireEvent_OnStockHealthRestored();
        }

        public void Restore(int value)
        {
            if (IsFull)
                return;

            current = (current + value) > maximum ? maximum : (current + value);

            _FireEvent_OnStockHealthValueChanged();
            _FireEvent_OnStockHealthRestored();
        }

        public void Remove(int value)
        {
            if (IsEmpty)
                return;

            current = (current - value) < 0 ? 0 : (current - value);

            _FireEvent_OnStockHealthValueChanged();
            _FireEvent_OnStockHealthRemoved();
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

        void _FireEvent_OnStockHealthRestored()
        {
            if (OnStockHealthRestored != null)
                OnStockHealthRestored();
        }

        void _FireEvent_OnStockHealthRemoved()
        {
            if (OnStockHealthRemoved != null)
                OnStockHealthRemoved();
        }
    }
}

