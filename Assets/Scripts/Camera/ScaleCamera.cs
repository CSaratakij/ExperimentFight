
using UnityEngine;
using UnityEngine.U2D;

namespace ExperimentFight
{
    public class ScaleCamera : MonoBehaviour
    {
        public static readonly Vector2Int REFERENCE_RESOLUTION = new Vector2Int(960, 540);

        PixelPerfectCamera pixelPerfectCamera;
        int cachePixelPerUnit;


        void Awake()
        {
            Initialize();
        }

        void LateUpdate()
        {
            AdjustCamera();
        }

        void Initialize()
        {
            pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
            pixelPerfectCamera.refResolutionX = REFERENCE_RESOLUTION.x;
            pixelPerfectCamera.refResolutionY = REFERENCE_RESOLUTION.y;
            cachePixelPerUnit = (Screen.height == 1080) ? 48 : 64;
        }

        void AdjustCamera()
        {
            if (pixelPerfectCamera == null)
                return;

            cachePixelPerUnit = (Screen.height == 1080) ? 48 : 64;

            if (cachePixelPerUnit == pixelPerfectCamera.assetsPPU)
                return;

            if (Screen.height == 1080)
                pixelPerfectCamera.assetsPPU = 48;
            else
                pixelPerfectCamera.assetsPPU = 64;
        }
    }
}
