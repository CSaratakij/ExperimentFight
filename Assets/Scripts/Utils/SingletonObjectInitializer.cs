using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class SingletonObjectInitializer : MonoBehaviour
    {
        public static SingletonObjectInitializer instance = null;

        [SerializeField]
        GameObject[] instances;

        void Awake()
        {
            MakeSingleton();
        }

        void MakeSingleton()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (GameObject obj in instances)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }
}
