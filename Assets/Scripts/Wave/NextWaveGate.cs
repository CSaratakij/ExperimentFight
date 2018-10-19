using UnityEngine;

namespace ExperimentFight
{
    public class NextWaveGate : MonoBehaviour
    {
        [SerializeField]
        WaveController waveController;

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
                Debug.Log("Next Wave Gate is enter by an unknown!?");
                return;
            }

            waveController.NextWave();
        }
    }
}
