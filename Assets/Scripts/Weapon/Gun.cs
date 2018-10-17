using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExperimentFight
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        bool isInfiniteClip = false;

        [SerializeField]
        float bulletForce;

        [SerializeField]
        Transform barrel;

        [SerializeField]
        [Range(1, 100)]
        int totalClip;

        [SerializeField]
        [Range(1, 100)]
        int maxClip;

        [SerializeField]
        [Range(1, 20)]
        int totalBulletPool;

        [SerializeField]
        GameObject bulletPrefab;

        bool isCanShoot;

        Vector2 aimAxis;
        Timer timer;

        Bullet[] bulletPool;


        void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        void OnDestroy()
        {
            UnsubscribeEvents();
        }

        void Initialize()
        {
            isCanShoot = true;

            timer = GetComponent<Timer>();
            bulletPool = new Bullet[totalBulletPool];

            for (int i = 0; i < totalBulletPool; ++i)
            {
                bulletPool[i] = (Instantiate(bulletPrefab) as GameObject).GetComponent<Bullet>();
                bulletPool[i].gameObject.SetActive(false);
            }
        }

        void AimAt(Vector2 direction)
        {
            aimAxis = direction;
        }

        void SubscribeEvents()
        {
            timer.OnStarted += OnTimerStarted;
            timer.OnStopped += OnTimerStopped;
        }

        void UnsubscribeEvents()
        {
            timer.OnStarted -= OnTimerStarted;
            timer.OnStopped -= OnTimerStopped;
        }

        void OnTimerStarted()
        {
            isCanShoot = false;
        }

        void OnTimerStopped()
        {
            isCanShoot = true;
        }

        public void Shoot(Vector2 direction)
        {
            AimAt(direction);
            Shoot();
        }

        public void Shoot()
        {
            if (!isCanShoot || totalClip <= 0)
                return;

            foreach (Bullet obj in bulletPool)
            {
                if (obj.gameObject.activeSelf)
                    continue;

                if (!isInfiniteClip)
                    totalClip -= 1;

                obj.transform.position = barrel.position;
                obj.moveDirection = aimAxis;
                obj.moveForce = bulletForce;
                obj.gameObject.SetActive(true);

                timer.CountDown();
                break;
            }
        }
        
        public void RestoreClip(int value)
        {
            totalClip = (totalClip + value) > maxClip ? maxClip : (totalClip + value);
        }

        public void FullRestoreClip()
        {
            totalClip = maxClip;
        }

        public void ClearClip()
        {
            totalClip = 0;
        }
    }
}
