using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace ExperimentFight
{
    public class Melee : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 10)]
        int maxDetectHit;

        [SerializeField]
        [Range(1, 3)]
        int maxMeleeChain;

        [SerializeField]
        Transform targetSlash;

        [SerializeField]
        Vector2 size;

        [SerializeField]
        LayerMask targetLayer;

        [SerializeField]
        Timer slashRateTimer;

        [SerializeField]
        Timer allowSlashTimer;

        [SerializeField]
        Timer frequentChainTimer;


        public bool IsAllowSlash { get { return isCanSlash; } }

        int totalHit;
        int totalMeleeChain;

        bool isCanSlash = true;
        SpriteRenderer spriteRenderer;

        Collider2D[] hits;
        IEnemy enemy;

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            if (targetSlash)
                Gizmos.DrawWireCube(targetSlash.position, new Vector3(size.x, size.y, 0.0f));
        }
#endif
        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void FixedUpdate()
        {
            totalHit = Physics2D.OverlapBoxNonAlloc(targetSlash.transform.position, size, 0.0f, hits, targetLayer);
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void Initialize()
        {
            hits = new Collider2D[maxDetectHit];
            spriteRenderer = targetSlash.GetComponent<SpriteRenderer>();
        }

        void OnTimerStart()
        {
            isCanSlash = false;
            spriteRenderer.color = Color.gray;
            totalMeleeChain += 1;
        }

        void OnTimerStop()
        {
            isCanSlash = true;
            spriteRenderer.color = Color.white;

            if (totalMeleeChain >= maxMeleeChain)
                allowSlashTimer.CountDown();
        }

        void OnAllowSlashTimerStop()
        {
            totalMeleeChain = 0;
        }

        void OnFrequentMeleeChainTimerStop()
        {
            if (totalMeleeChain < maxMeleeChain)
                totalMeleeChain = 0;
        }

        void SubscribeEvents()
        {
            slashRateTimer.OnStarted += OnTimerStart;
            slashRateTimer.OnStopped += OnTimerStop;

            allowSlashTimer.OnStopped += OnAllowSlashTimerStop;
            frequentChainTimer.OnStopped += OnFrequentMeleeChainTimerStop;
        }

        void UnsubscribeEvents()
        {
            slashRateTimer.OnStarted -= OnTimerStart;
            slashRateTimer.OnStopped -= OnTimerStop;

            allowSlashTimer.OnStopped -= OnAllowSlashTimerStop;
            frequentChainTimer.OnStopped -= OnFrequentMeleeChainTimerStop;
        }

        public void Slash()
        {
            if (!isCanSlash)
                return;

            if (totalMeleeChain >= maxMeleeChain)
                return;

            isCanSlash = false;

            slashRateTimer.CountDown();
            frequentChainTimer.CountDown();

            if (totalHit <= 0)
                return;

            for (int i = 0; i < totalHit; ++i)
            {
                if (hits[i] == null)
                    continue;

                enemy = hits[i].GetComponent<IEnemy>();

                if (enemy != null)
                    enemy.GetHit();
            }
        }
    }
}
