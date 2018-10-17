using UnityEngine;

namespace ExperimentFight
{
    public class Slime : MonoBehaviour
    {
        bool isCanMove = true;

        Timer timer;

        SpriteRenderer spriteRenderer;
        StockHealth health;

        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void FixedUpdate()
        {
            MoveHandler();
        }

        void Initialize()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            health = GetComponent<StockHealth>();
            timer = GetComponent<Timer>();
        }

        void MoveHandler()
        {
            if (!isCanMove)
            {

                return;
            }


        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider)
                return;

            if (collider.CompareTag("PlayerBullet"))
                timer.CountDown();
        }

        void OnTimerStart()
        {
            isCanMove = false;
            spriteRenderer.color = Color.red;
        }

        void OnTimerStop()
        {
            isCanMove = true;
            spriteRenderer.color = Color.white;

            health.Remove(1);

            if (health.IsEmpty)
                gameObject.SetActive(false);
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
    }
}
