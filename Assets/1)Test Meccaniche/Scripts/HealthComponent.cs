using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public class HealthComponent : MonoBehaviour
    {
        private float maxHealth;
        private float currentHealth;
        private bool canTakeDamage;

        // Start is called before the first frame update
        void Start()
        {
            //controllo se ho lo scudo
            canTakeDamage = true;
            //prendo da file la vita massima
            maxHealth = 10;
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentHealth <= 0)
            {
                Debug.Log("Uaglio Ociuccio è morto");
            }
        }

        public void HealMe(float healAmount)
        {
            currentHealth = MathF.Max(healAmount + currentHealth, maxHealth);
        }

        public void TakeDamage(float damageAmount)
        {
            if (canTakeDamage)
            {
                currentHealth -= damageAmount;
            }
        }

        public void IncreaseMaxHealth(float healthIncreaseValue)
        {
            if (tag == "Player")
            {
                maxHealth += healthIncreaseValue;
                currentHealth = maxHealth;
            }
        }
    }
}
