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
            directionTarget = GameObject.Find("ShootTarget").transform;
        }

        private void Update()
        {
            
            if(directionTarget.parent != null)
            {
                transform.position = Vector3.MoveTowards(transform.position,directionTarget.position,speed * Time.deltaTime);
            }else
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            bulletTimer += Time.deltaTime;
            if(bulletTimer >= bulletLifeTime)
            {
                gameObject.SetActive(false);
                bulletTimer = 0;
            }

        } 

        private void OnCollisionEnter(Collision collision)
        {
            //fare danno al nemico
        }

    }
}