using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheChroniclesOfEllen
{
    public class ShootComponent : MonoBehaviour
    {
        public Transform mouthOfFire;
        public int damage;
        private Bullet bullet;
        public float shootCD = 0.5f;
        private float currentTimer;
        private float overheat = 10;
        private float currentOverheat;
        private float overheatPerBullet = 1.5f;
        private PlayerPowerUp powerUpSystem;
        public bool isSpitter;

        // Start is called before the first frame update
        void Start()
        {
            currentTimer = 0;
            currentOverheat = 0;

            if(!isSpitter)
                powerUpSystem = GetComponentInParent<PlayerPowerUp>();
        }

        // Update is called once per frame
        void Update()
        {
            currentTimer += Time.deltaTime;
            currentOverheat = Mathf.Max(currentOverheat - Time.deltaTime, 0);
        }

        public void OnShoot(Transform target)
        {
            if (currentTimer > shootCD && currentOverheat <= overheat)
            {
                if (isSpitter)
                {
                    bullet = BulletPool.GetBulletEnemy();
                }
                else if (!isSpitter && powerUpSystem.HaveSpecialLeft()) 
                {
                    bullet = BulletPool.GetBulletSpecial();
                }
                else
                {
                    bullet = BulletPool.GetBullet();
                }
                
                //if (target != null)
                //{
                //    bullet.SetTarget(target);
                //}
                currentOverheat += overheatPerBullet;
                currentTimer = 0;
                bullet.transform.position = mouthOfFire.position;
                bullet.damage = damage;
                bullet.speed = 10;
                bullet.direction = mouthOfFire.forward;
            }
            
        }

        public void OnShoot(Vector3 direction)
        {
            if (currentTimer > shootCD && currentOverheat <= overheat)
            {
                if (isSpitter)
                {
                    bullet = BulletPool.GetBulletEnemy();
                }
                else if (!isSpitter && powerUpSystem.HaveSpecialLeft())
                {
                    bullet = BulletPool.GetBulletSpecial();
                }
                else
                {
                    bullet = BulletPool.GetBullet();
                }

                bullet.direction = direction;
                currentOverheat += overheatPerBullet;
                currentTimer = 0;
                bullet.transform.position = mouthOfFire.position;
                bullet.damage = damage;
                bullet.speed = 10;
            }

        }
    }
}  