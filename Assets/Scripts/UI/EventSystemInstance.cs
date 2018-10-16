using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExperimentFight
{
    public class EventSystemInstance : MonoBehaviour
    {
        public static EventSystemInstance instance = null;
        public EventSystem eventSystem;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                eventSystem = GetComponent<EventSystem>();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
