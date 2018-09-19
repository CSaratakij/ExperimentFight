
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        Vector3 offset;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        float interpolateAmount;


        Vector3 newPos;


        void LateUpdate()
        {
            newPos = (target.position + offset);
            newPos = Vector3.Lerp(transform.position, newPos, interpolateAmount);
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        public void SetOffset(Vector3 value)
        {
            offset = value;
        }

        public void SetInterpolateAmount(float value)
        {
            interpolateAmount = value;
        }

        public void ResetOffset()
        {
            offset = Vector3.zero;
        }
    }
}
