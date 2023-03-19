using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class HealthComponent : MonoBehaviour
    {
        private float maxHealth;
        private float currentHealth;
        private bool shieldActive;
        private PlayerPowerUp powerUpSystem;
        public Transform shield;
        private bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
        }

        // Start is called before the first frame update
        void Start()
        {
            //controllo se ho lo scudo
            isAlive = true; 
            shield.gameObject.SetActive(false);
            //prendo da file la vita massima
            maxHealth = 10;
            currentHealth = maxHealth;
            powerUpSystem = GetComponent<PlayerPowerUp>();
        }

        // Update is called once per frame
        void Update()
        {
            shield.gameObject.SetActive(shieldActive);
            if (currentHealth <= 0)
            {
                isAlive = false;
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
            Debug.Log("Colpito");
            shieldActive = powerUpSystem.IsShieldActive();
            if (!shieldActive)
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
