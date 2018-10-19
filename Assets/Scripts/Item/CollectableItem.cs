using UnityEngine;

namespace ExperimentFight
{
    public class CollectableItem : MonoBehaviour
    {
        const float MAX_PROGRESS = 1.0f;

        [SerializeField]
        bool holdDownKeyToCollect = true;

        [SerializeField]
        [Range(1.0f, 2.0f)]
        float progressRate = 1.0f;

        [SerializeField]
        Transform uiProgressPos;

        public delegate void _Func(GameObject collector);
        public event _Func OnCollectItem;

        GameObject collector = null;

        bool isDetectPlayer = false;
        bool isUsed = false;

        float currentProgress = 0.0f;

        void OnDisable()
        {
            isUsed = false;
            currentProgress = 0.0f;
        }

        void OnDestroy()
        {
            OnCollectItem = null;
        }

        void Update()
        {
            CollectItemHandler();
        }

        void CollectItemHandler()
        {
            if (isUsed)
                return;

            if (!isDetectPlayer)
                return;

            if (holdDownKeyToCollect)
                HoldToCollectItem();
            else
                PressKeyDownToCollectItem();

            UIProgress.instance.SetProgress(currentProgress);
        }

        void HoldToCollectItem()
        {
            if (Input.GetButton("CollectItem"))
            {
                currentProgress += (progressRate * Time.deltaTime);

                if (currentProgress >= MAX_PROGRESS)
                    CollectItem();
            }
            else
                currentProgress = 0.0f;
        }

        void PressKeyDownToCollectItem()
        {
            if (Input.GetButtonDown("CollectItem"))
            {
                currentProgress = MAX_PROGRESS;
                CollectItem();
            }
            else
                currentProgress = 0.0f;
        }

        void CollectItem()
        {
            isUsed = true;

            if (OnCollectItem != null)
                OnCollectItem(collector);

            UIProgress.instance.SetProgress(0);
            UIProgress.instance.Show(false);

            gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isDetectPlayer = true;
                collector = collision.gameObject;
                UIProgress.instance.Show(true, uiProgressPos.position);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isDetectPlayer = false;
                collector = null;
                UIProgress.instance.Show(false);
            }
        }
    }
}
