using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class EnemyHitBox : MonoBehaviour
    {
        public bool isAttacking;
        public float damage;

        private void OnTriggerEnter(Collider other)
        {
            if (isAttacking)
            {
                Debug.Log(gameObject.name);
                other.gameObject.GetComponent<HealthComponent>().TakeDamage(damage);
            }
        }
    }

}