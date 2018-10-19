using UnityEngine;

namespace ExperimentFight
{
    public class WaveController : MonoBehaviour
    {
        public delegate void _Func();
        public event _Func OnNextWave;

        public int CurrentWave { get; private set; }

        [SerializeField]
        GameObject healthKit;

        [SerializeField]
        GameObject enemyPrefab;

        [SerializeField]
        GameObject nextWaveGate;

        [SerializeField]
        Transform[] possibleEnemySpawnPoints;

        [SerializeField]
        int maxSpawn = 10;

        [SerializeField]
        int increaseSpawnStep = 2;


        Timer timer;

        bool isCanNextWave = false;
        bool isShowNextWaveGate = false;

        int currentSpawn;
        GameObject[] enemies;


        void Awake()
        {
            timer = GetComponent<Timer>();
            CurrentWave = 0;

            enemies = new GameObject[maxSpawn];
            for (int i = 0; i < maxSpawn; ++i)
            {
                enemies[i] = Instantiate(enemyPrefab) as GameObject;
                enemies[i].SetActive(false);
            }

            SubscribeEvents();
        }

        void Update()
        {
            if (!GameController.IsGameStart)
                return;

            if (isShowNextWaveGate)
                return;

            int deadCount = 0;

            for (int i = 0; i < currentSpawn; ++i)
            {
                if (enemies[i].activeSelf)
                    continue;
                else
                    deadCount += 1;
            }

            if (deadCount == currentSpawn)
            {
                isShowNextWaveGate = true;
                nextWaveGate.SetActive(true);
                timer.Pause(true);
            }
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void SubscribeEvents()
        {
            GameController.OnGameStart += OnGameStart;
            GameController.OnGameOver += OnGameOver;
            timer.OnStopped += OnTimerStop;
        }

        void UnsubscribeEvents()
        {
            GameController.OnGameStart -= OnGameStart;
            GameController.OnGameOver -= OnGameOver;

            timer.OnStopped -= OnTimerStop;
            OnNextWave = null;
        }

        void OnTimerStop()
        {
            GameController.GameStop();
        }

        void OnGameStart()
        {
            isCanNextWave = true;
            NextWave();
        }

        void OnGameOver()
        {
            isCanNextWave = false;
            isShowNextWaveGate = false;
            nextWaveGate.SetActive(false);
            timer.Stop();
        }

        void ActivateAllEnemy()
        {
            for (int i = 0; i < currentSpawn; ++i)
            {
                enemies[i].transform.position = possibleEnemySpawnPoints[i].position;
            }

            for (int i = 0; i < currentSpawn; ++i)
            {
                enemies[i].SetActive(true);
            }
        }

        public void NextWave()
        {
            if (!isCanNextWave)
                return;

            CurrentWave += 1;
            timer.Reset();

            if (OnNextWave != null)
                OnNextWave();

            healthKit.SetActive(true);

            isShowNextWaveGate = false;
            nextWaveGate.SetActive(false);

            if (currentSpawn < maxSpawn)
                currentSpawn += increaseSpawnStep;

            ActivateAllEnemy();
            timer.CountDown();
        }
    }
}
