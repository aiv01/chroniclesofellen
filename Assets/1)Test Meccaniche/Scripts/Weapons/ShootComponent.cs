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
        private float bulletTimer;
        private float bulletLifeTime = 3.5f;
        private float surriscaldamento = 10;
        private float surriscaldamentoAttuale;
        private float surriscaldamentoPerColpo = 1.5f;

        // Start is called before the first frame update
        void Start()
        {
            currentTimer = 0;
            surriscaldamentoAttuale = 0;
        }

        // Update is called once per frame
        void Update()
        {
            
            surriscaldamentoAttuale = Mathf.Max(surriscaldamentoAttuale - Time.deltaTime, 0);
        }

        public void Shoot()
        {
            currentTimer += Time.deltaTime;
            
            if (currentTimer > shootCD && surriscaldamentoAttuale <= surriscaldamento) 
            {
                surriscaldamentoAttuale += surriscaldamentoPerColpo;
                currentTimer = 0;
                bullet = BulletPool.GetBullet();
                bullet.transform.position = mouthOfFire.position;
                bullet.damage = damage;
                bullet.speed = 10;
                bullet.direction = mouthOfFire.forward;
                
                

            }
            
        }


    }
}  