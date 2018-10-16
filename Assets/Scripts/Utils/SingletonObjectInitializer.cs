using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class SingletonObjectInitializer : MonoBehaviour
    {
        [SerializeField]
        GameObject[] instances;

        void Awake()
        {
            MakeSingleton();
        }

        void MakeSingleton()
        {
            foreach (GameObject obj in instances)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }
}
