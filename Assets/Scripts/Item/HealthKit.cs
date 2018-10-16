using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    [RequireComponent(typeof(CollectableItem))]
    public class HealthKit : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 4)]
        int totalRestore = 1;

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

        void OnCollectItem(GameObject collector)
        {
            if (!collector)
            {
                Debug.Log("Health kit is collect by an unknown collector!?");
                return;
            }

            StockHealth health = collector.GetComponent<StockHealth>();
            health.Restore(totalRestore);
        }
    }
}
