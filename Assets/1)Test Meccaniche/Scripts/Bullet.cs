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

        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        } 

        private void OnCollisionEnter(Collision collision)
        {
            //fare danno al nemico
        }

    }
}