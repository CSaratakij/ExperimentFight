using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class GamepadWatcher : MonoBehaviour
    {
        public static bool isGamepadConnected = false;

        [SerializeField]
        bool lessFrequent;


        void Update()
        {
            if (lessFrequent) {
                if (Input.anyKeyDown) {
                    _CheckConnectedGamepad();
                }
            }
            else {
                _CheckConnectedGamepad();
            }
        }

        void _CheckConnectedGamepad()
        {
            foreach (string name in Input.GetJoystickNames()) {
                if (string.IsNullOrEmpty(name)) {
                    isGamepadConnected = false;
                    continue;
                }
                else
                {
                    isGamepadConnected = true;
                    break;
                }
            }
        }
    }
}
