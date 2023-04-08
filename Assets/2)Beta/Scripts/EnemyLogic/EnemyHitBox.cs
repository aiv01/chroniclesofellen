using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class EnemyHitBox : MonoBehaviour
    {
        public bool isAttacking;
        public int damage;

        private void OnTriggerEnter(Collider other)
        {
            if (isAttacking)
            {
                other.gameObject.GetComponent<HealthComponent>().TakeDamage(damage);
            }
        } 
    }

}