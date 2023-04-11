using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheChroniclesOfEllen
{
    public class Bullet : MonoBehaviour
    {
        public int damage;
        [SerializeField]
        public float speed = 100f;
        public Vector3 direction;
        private Transform directionTarget;
        private float bulletTimer;
        private float bulletLifeTime = 3.5f;
        
        private void Start()
        {
            bulletTimer = 0;
        }

        private void Update()
        {

            transform.position += direction * speed * Time.deltaTime;
            
            bulletTimer += Time.deltaTime;
            if (bulletTimer >= bulletLifeTime)
            {
                gameObject.SetActive(false);
                bulletTimer = 0;
            }

        }

        public void SetTarget(Transform target)
        {
            if (directionTarget != null)
                return;
            directionTarget = target;
        }


        private void OnCollisionEnter(Collision collision)
        {
            bulletTimer = 0;
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Boss")
            {
                collision.gameObject.GetComponent<HealthComponent>().TakeDamage(damage);
                
            }
            
            if(gameObject.name == "PlayerBullet" || gameObject.name == "PlayerSpecialBullet")
            {
                return;
            }
            

        }
    }
}