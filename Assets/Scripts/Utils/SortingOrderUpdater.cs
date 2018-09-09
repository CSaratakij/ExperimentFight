
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class SortingOrderUpdater : MonoBehaviour
    {
        int cacheSortingOrder;
        int oldSortingOrder;

        SpriteRenderer spriteRenderer;


        void Awake()
        {
            Initialize();
        }

        void LateUpdate()
        {
            UpdateSortingOrder();
        }

        void Initialize()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void UpdateSortingOrder()
        {
            oldSortingOrder = cacheSortingOrder;
            cacheSortingOrder = (int)(transform.position.y * -10);

            if (cacheSortingOrder == oldSortingOrder)
                return;

            spriteRenderer.sortingOrder = cacheSortingOrder;
        }
    }
}

