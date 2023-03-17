using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;


namespace TheChroniclesOfEllen
{
    public class Bullet : MonoBehaviour
    {
        public int damage;
        public float speed;
        public Vector3 direction;
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