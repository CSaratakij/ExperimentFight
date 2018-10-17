using UnityEngine;

namespace ExperimentFight
{
    public class Bullet : MonoBehaviour
    {
        public bool IsCanMove { get; set; }
        public Vector2 moveDirection { get; set; }
        public float moveForce { get; set; }

        Rigidbody2D rigid;

        void OnEnable()
        {
            IsCanMove = true;
        }

        void OnDisable()
        {
            IsCanMove = false;
        }

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            moveDirection = Vector2.right;
        }

        void FixedUpdate()
        {
            MoveHandler();
        }

        void MoveHandler()
        {
            if (!IsCanMove)
            {
                rigid.velocity = Vector2.zero;
                return;
            }

            rigid.velocity = (moveDirection * moveForce) * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider)
                return;

            if (collider.CompareTag("Bullet"))
                return;

            gameObject.SetActive(false);
        }
    }
}
