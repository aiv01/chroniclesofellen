using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnShiedDamage;
        private float maxHealth;
        private float currentHealth;
        private bool isShieldActive;

        // Start is called before the first frame update
        void Start()
        {
            //controllo se ho lo scudo
            isShieldActive = true;
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
            Debug.Log("Ociuccio è curato di " + healAmount);
            currentHealth = MathF.Max(healAmount + currentHealth, maxHealth);
        }

        public void TakeDamage(float damageAmount)
        {
            if (!isShieldActive)
            {
                currentHealth -= damageAmount;
            }
            else
            {
                OnShiedDamage.Invoke();
            }
        }

        public void ManageShield(bool shieldStatus)
        {
            isShieldActive = !shieldStatus;
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
