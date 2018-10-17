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
        Transform targetSlash;

        [SerializeField]
        Vector2 size;

        [SerializeField]
        LayerMask targetLayer;


        public bool IsAllowSlash { get { return isCanSlash; } }

        int totalHit;
        bool isCanSlash = true;

        Timer timer;
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
            timer = GetComponent<Timer>();
            spriteRenderer = targetSlash.GetComponent<SpriteRenderer>();
        }

        void OnTimerStart()
        {
            isCanSlash = false;
            spriteRenderer.color = Color.gray;
        }

        void OnTimerStop()
        {
            isCanSlash = true;
            spriteRenderer.color = Color.white;
        }

        void SubscribeEvents()
        {
            timer.OnStarted += OnTimerStart;
            timer.OnStopped += OnTimerStop;
        }

        void UnsubscribeEvents()
        {
            timer.OnStarted -= OnTimerStart;
            timer.OnStopped -= OnTimerStop;
        }

        public void Slash()
        {
            if (!isCanSlash)
                return;

            isCanSlash = false;
            timer.CountDown();

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
