
using System.Collections;
using System.Collections.Generic;
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


        bool isDashing;

        Vector2 inputVector;
        Vector2 lastInputVector;

        Vector2 velocity;
        Vector2 moveVector;

        Animator anim;
        Rigidbody2D rigid;
        StockHealth stockHealth;
        Timer timer;


        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void Update()
        {
            //Test
            /* if (Input.GetButtonDown("Jump")) */
            /*     stockHealth.Remove(1); */

            InputHandler();
            AnimationHandler();
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
            isDashing = false;
            inputVector = Vector2.zero;
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            stockHealth = GetComponent<StockHealth>();
            timer = GetComponent<Timer>();
        }

        void InputHandler()
        {
            if (stockHealth.IsEmpty) {
                inputVector = Vector2.zero;
                return;
            }

            if (isDashing)
                return;

            if (Input.GetButtonDown("Jump") && !isDashing) {
                isDashing = true;
                //Need fix
                inputVector = lastInputVector; // if player is not moving input vector (0, 0) -> make this the facing vector
                timer.CountDown();
            }

            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.y = Input.GetAxisRaw("Vertical");

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
            velocity = (moveForce * inputVector) * Time.fixedDeltaTime;
            rigid.velocity = velocity;
        }

        void DashHandler()
        {
            velocity = (dashForce * lastInputVector) * Time.fixedDeltaTime;
            rigid.velocity = velocity;
            //rigid.AddForce(velocity, ForceMode2D.Force);
        }

        void SubscribeEvents()
        {
            timer.OnStopped += OnTimerStopped;
        }

        void UnsubscribeEvents()
        {
            timer.OnStopped -= OnTimerStopped;
        }

        void OnTimerStopped()
        {
            isDashing = false;
        }
    }
}

