using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    [RequireComponent(typeof(CollectableItem))]
    public class HealthKit : MonoBehaviour
    {
        CollectableItem item;

        void Awake()
        {
            item = GetComponent<CollectableItem>();
            SubscribeEvents();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void SubscribeEvents()
        {
            item.OnCollectItem += OnCollectItem;
        }

        void UnsubscribeEvents()
        {
            item.OnCollectItem -= OnCollectItem;
        }

        void OnCollectItem()
        {
            Debug.Log("Health is collect...");
        }
    }
}
