
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        bool isPause;

        [SerializeField]
        float current;

        [SerializeField]
        float maximum;


        public bool IsStart { get { return isStart; } }
        public bool IsPause { get { return isPause; } }

        public float Current { get { return current; } }
        public float Maximum { get { return maximum; } }


        public delegate void _Func();

        public event _Func OnStarted;
        public event _Func OnStopped;


        bool isStart;


        void OnDestroy()
        {
            OnStopped = null;
        }

        void Update()
        {
            if (!isStart)
                return;

            if (isPause)
                return;

            current -= (1.0f * Time.deltaTime);

            if (current <= 0.0f)
                Stop();
        }

        public void CountDown()
        {
            if (isStart)
                return;

            isStart = true;
            current = maximum;
            _FireEvent_OnStarted();
        }

        public void Pause(bool value)
        {
            isPause = value;
        }

        public void Stop()
        {
            if (!isStart)
                return;

            isStart = false;
            isPause = false;
            current = 0.0f;

            _FireEvent_OnStopped();
        }

        public void Reset()
        {
            current = maximum;
            isStart = false;
            isPause = false;
        }

        void _FireEvent_OnStarted()
        {
            if (OnStarted != null)
                OnStarted();
        }

        void _FireEvent_OnStopped()
        {
            if (OnStopped != null)
                OnStopped();
        }
    }
}

