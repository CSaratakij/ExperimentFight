using UnityEngine;
using UnityEngine.UI;

namespace ExperimentFight
{
    public class WaveUIController : MonoBehaviour
    {
        const string WAVE_FORMAT = "Wave : {0}";
        const string WAVE_TIMER_FORMAT = "Time Left : {0}:{1}";

        [SerializeField]
        WaveController waveController;

        [SerializeField]
        Text lblWave;

        [SerializeField]
        Text lblWaveTimer;

        [SerializeField]
        Timer waveTimer;


        void Awake()
        {
            SubscribeEvents();
        }

        void Update()
        {
            UpdateWaveTimer();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void SubscribeEvents()
        {
            waveController.OnNextWave += OnNextWave;
        }

        void UnsubscribeEvents()
        {
            waveController.OnNextWave -= OnNextWave;
        }

        void UpdateWaveTimer()
        {
            int minute = (int)(waveTimer.Current / 60);
            int seconds = (int)(waveTimer.Current % 60);

            lblWaveTimer.text = string.Format(WAVE_TIMER_FORMAT, minute.ToString("00"), seconds.ToString("00"));
        }

        void OnNextWave()
        {
            lblWave.text = string.Format(WAVE_FORMAT, waveController.CurrentWave);
        }
    }
}
