using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    [RequireComponent(typeof(StockHealth))]
    public class DamageAbleObject : MonoBehaviour
    {
        StockHealth health;
        Animator anim;

        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void Initialize()
        {
            health = GetComponent<StockHealth>();
            anim = GetComponent<Animator>();
        }

        void SubscribeEvents()
        {
            health.OnStockHealthRemoved += OnReceivedDamage;
        }

        void UnsubscribeEvents()
        {
            health.OnStockHealthRemoved -= OnReceivedDamage;
        }

        void OnReceivedDamage()
        {
            /*
            if (health.IsEmpty)
                anim.Play("Destroyed"); //after animation finished -> disable game object...
            else
                anim.Play("Damaged");
            */
        }

        public void DisableGameObject()
        {
            //call from animation...
            gameObject.SetActive(false);
        }
    }
}

