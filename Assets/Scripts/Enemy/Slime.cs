using UnityEngine;

namespace ExperimentFight
{
    public class Slime : MonoBehaviour, IEnemy
    {
        [SerializeField]
        float moveForce;

        [SerializeField]
        float chargeForce;

        [SerializeField]
        Timer stuntTimer;

        [SerializeField]
        Timer attackTimer;

        bool isCanMove = true;
        bool isStunt = false;
        bool isCharge = false;

        SpriteRenderer spriteRenderer;
        StockHealth health;

        Rigidbody2D rigid;
        Vector2 moveDirection;

        GameObject player;


        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void Update()
        {
            if (player == null)
                return;

            if (!isCharge)
                moveDirection = (player.transform.position - transform.position);
        }

        void FixedUpdate()
        {
            MoveHandler();
        }

        void Initialize()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            health = GetComponent<StockHealth>();
            stuntTimer = GetComponent<Timer>();
            rigid = GetComponent<Rigidbody2D>();

            player = GameObject.FindGameObjectWithTag("Player");
        }

        void MoveHandler()
        {
            if (!isCanMove || isStunt)
            {
                rigid.velocity = Vector2.zero;
                return;
            }

            if (moveDirection.magnitude > 1.0f)
                moveDirection = moveDirection.normalized;

            if (isCharge)
                rigid.velocity = (moveDirection * chargeForce) * Time.deltaTime;
            else
            {
                rigid.velocity = (moveDirection * moveForce) * Time.deltaTime;

                if (Vector2.Distance(player.transform.position, transform.position) < 4.0f)
                    attackTimer.CountDown();
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider)
                return;

            if (collider.CompareTag("PlayerBullet"))
                stuntTimer.CountDown();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null)
                return;

            if (collision.gameObject.CompareTag("Player") && !isStunt && isCharge)
            {
                IPlayer player = collision.gameObject.GetComponent<IPlayer>();
                player.GetHit();
            }

            if (isCharge)
                isCharge = false;
        }

        void OnTimerStart()
        {
            isCanMove = false;
            isStunt = true;

            spriteRenderer.color = Color.red;
        }

        void OnTimerStop()
        {
            isCanMove = true;
            isStunt = false;

            spriteRenderer.color = Color.white;
            health.Remove(1);

            if (health.IsEmpty)
                gameObject.SetActive(false);
        }

        void OnAttackTimerStart()
        {
            isCanMove = false;
        }

        void OnAttackTimerStop()
        {
            isCanMove = true;
            isCharge = true;

        }

        void SubscribeEvents()
        {
            stuntTimer.OnStarted += OnTimerStart;
            stuntTimer.OnStopped += OnTimerStop;

            attackTimer.OnStarted += OnAttackTimerStart;
            attackTimer.OnStopped += OnAttackTimerStop;
        }

        void UnsubscribeEvents()
        {
            stuntTimer.OnStarted -= OnTimerStart;
            stuntTimer.OnStopped -= OnTimerStop;

            attackTimer.OnStarted -= OnAttackTimerStart;
            attackTimer.OnStopped -= OnAttackTimerStop;
        }

        public void GetHit()
        {
            isStunt = true;
            stuntTimer.CountDown();
        }
    }
}
