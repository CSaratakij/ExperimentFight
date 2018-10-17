using UnityEngine;

namespace ExperimentFight
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float moveForce;

        [SerializeField]
        float dashForce;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        float inputVectorDeadzone;

        [SerializeField]
        Timer allowDashTimer;

        [SerializeField]
        Timer dashTimer;

        [SerializeField]
        Gun gun;

        [SerializeField]
        Transform barrelOfGun;

        [SerializeField]
        Transform aimUI;

        [SerializeField]
        Melee melee;

        public bool IsInvincible { get; private set; }

        bool isLockingOn;
        bool isAllowDash;

        bool isDashing;
        bool isSlashing;

        Vector2 inputVector;
        Vector2 lastInputVector;

        Vector2 velocity;
        Vector2 moveVector;

        SpriteRenderer spriteRenderer;
        Animator anim;

        Rigidbody2D rigid;
        StockHealth stockHealth;

        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void Update()
        {
            InputHandler();
            AnimationHandler();
        }

        void LateUpdate()
        {
            ChangePositionOfGunBarrel(-lastInputVector);
        }

        void FixedUpdate()
        {
            if (isDashing)
                DashHandler();
            else
                MovementHandler();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void Initialize()
        {
            isAllowDash = true;
            isDashing = false;

            inputVector = Vector2.zero;

            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();

            rigid = GetComponent<Rigidbody2D>();
            stockHealth = GetComponent<StockHealth>();

            lastInputVector = Vector2.down;
        }

        void InputHandler()
        {
            if (GameController.gameState == GameState.Pause)
                return;

            if (stockHealth.IsEmpty) {
                inputVector = Vector2.zero;
                return;
            }

            if (isDashing)
                return;

            if (Input.GetButtonDown("Dash") && isAllowDash && !isDashing && !isSlashing) {
                isAllowDash = false;
                isDashing = true;
                IsInvincible = true;
                inputVector = lastInputVector;
                spriteRenderer.color = Color.gray;
                dashTimer.CountDown();
            }

            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.y = Input.GetAxisRaw("Vertical");

            InputProcessing();

            isLockingOn = Input.GetButton("LockOn");

            if (aimUI.gameObject.activeSelf != isLockingOn)
                aimUI.gameObject.SetActive(isLockingOn);

            if (isLockingOn && Input.GetButtonDown("Shoot"))
                gun.Shoot(barrelOfGun.transform.up);

            if (Input.GetButtonDown("Slash") && !isLockingOn)
                melee.Slash();

            isSlashing = !melee.IsAllowSlash;
        }

        void InputProcessing()
        {
            if (inputVector.x > inputVectorDeadzone)
                inputVector.x = 1.0f;

            else if (inputVector.x < -inputVectorDeadzone)
                inputVector.x = -1.0f;

            else
                inputVector.x = 0.0f;

            if (inputVector.y > inputVectorDeadzone)
                inputVector.y = 1.0f;

            else if (inputVector.y < -inputVectorDeadzone)
                inputVector.y = -1.0f;

            else
                inputVector.y = 0.0f;

            if (inputVector.magnitude > 1.0f)
                inputVector = inputVector.normalized;

            if (inputVector.magnitude > 0.0f) 
                lastInputVector = inputVector;
        }

        void AnimationHandler()
        {
            if (stockHealth.IsEmpty) {
                anim.Play("Dead");
                return;
            }

            if (inputVector.magnitude <= 0.0f) {

                if (lastInputVector.x > 0.0f)
                    anim.Play("IdleRight");

                else if (lastInputVector.x < 0.0f)
                    anim.Play("IdleLeft");

                else if (lastInputVector.y > 0.0f)
                    anim.Play("IdleUp");

                else if (lastInputVector.y < 0.0f)
                    anim.Play("IdleDown");

                return;
            }

            if (lastInputVector.x > 0.0f)
                anim.Play("RunRight");

            else if (lastInputVector.x < 0.0f)
                anim.Play("RunLeft");

            else if (lastInputVector.y > 0.0f)
                anim.Play("RunUp");

            else if (lastInputVector.y < 0.0f)
                anim.Play("RunDown");
        }

        void MovementHandler()
        {
            if (isLockingOn || isSlashing)
            {
                rigid.velocity = Vector2.zero;
                return;
            }

            velocity = (moveForce * inputVector) * Time.fixedDeltaTime;
            rigid.velocity = velocity;
        }

        void DashHandler()
        {
            velocity = (dashForce * lastInputVector) * Time.fixedDeltaTime;
            rigid.velocity = velocity;
        }

        void ChangePositionOfGunBarrel(Vector2 direction)
        {
            Vector2 currentPos = transform.position;
            Vector2 relativeVector = (currentPos + direction) - currentPos;

            float angle = Mathf.Atan2(relativeVector.y, relativeVector.x);
            float degreeAngle = angle * (180 / Mathf.PI);

            degreeAngle += 90;

            gun.transform.localEulerAngles = (Vector3.forward * degreeAngle);
            melee.transform.localEulerAngles = (Vector3.forward * degreeAngle);
        }

        void SubscribeEvents()
        {
            allowDashTimer.OnStopped += OnAllowDashTimer_Stopped;
            dashTimer.OnStopped += OnDashTimer_Stopped;
        }

        void UnsubscribeEvents()
        {
            allowDashTimer.OnStopped -= OnAllowDashTimer_Stopped;
            dashTimer.OnStopped -= OnDashTimer_Stopped;
        }

        void OnAllowDashTimer_Stopped()
        {
            isAllowDash = true;
        }

        void OnDashTimer_Stopped()
        {
            isDashing = false;
            IsInvincible = false;
            spriteRenderer.color = Color.white;
            allowDashTimer.CountDown();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsInvincible)
                return;

            if (!collider)
                return;

            if (collider.CompareTag("EnemyBullet"))
            {
                stockHealth.Remove(1);
                spriteRenderer.color = Color.red;
            }
        }
    }
}
