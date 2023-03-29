using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheChroniclesOfEllen
{
    public class Bullet : MonoBehaviour
    {
        public int damage;
        public float speed;
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
            
            
            transform.position += directionTarget.position * speed * Time.deltaTime;
            
            bulletTimer += Time.deltaTime;
            if(bulletTimer >= bulletLifeTime)
            {
                gameObject.SetActive(false);
                directionTarget = null;
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
            Debug.Log("Sono morto");
            if (collision.gameObject.tag == "Player")
            {
                bulletTimer = bulletLifeTime;
                directionTarget = null;
                gameObject.SetActive(false);
            }
        }
    }
}