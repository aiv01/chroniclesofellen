using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace TheChroniclesOfEllen
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnPoiseBreak;
        public TextMeshProUGUI text;
        private float maxHealth;
        public float currentHealth;
        private bool shieldActive;
        private float maxPoise;
        private float currPoise;
        private float regenPoisePerSec;
        private PlayerPowerUp powerUpSystem;
        public Transform shield;
        private bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
        }

        public float HealthPerc
        {
            get { return currentHealth / maxHealth; }
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

            currPoise = maxPoise;
        }

        // Update is called once per frame
        void Update()
        {
            currPoise = MathF.Min(currPoise + regenPoisePerSec * Time.deltaTime, maxPoise);

            if(tag == "Player")
            {
                text.text = currentHealth + " / " + maxHealth;
                shield.gameObject.SetActive(powerUpSystem.IsShieldActive());
            }
            if (currentHealth <= 0)
            {
                isAlive = false;
                currentHealth = 0;
            }
        }

        public void SetMaxHealth(float maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
        }

        public void HealMe(float healAmount)
        {
            currentHealth = MathF.Min(healAmount + currentHealth, maxHealth);
        }

        public void TakeDamage(float damageAmount)
        {
            if (tag == "Player")
            {
                if (powerUpSystem.OnHit())
                {
                    return;
                }
            }
            currPoise -= damageAmount;

            if (currPoise <= 0)
            {
                currPoise = maxPoise;
                OnPoiseBreak.Invoke();
            }

            currentHealth -= damageAmount;
            Debug.Log(currentHealth);
        }

        public void IncreaseMaxHealth(float healthIncreaseValue)
        {
            if (tag == "Player")
            {
                maxHealth += healthIncreaseValue;
                currentHealth = maxHealth;
            }
        }

        public void Shield(bool shieldStatus)
        {
            Debug.Log(shieldStatus);
            shield.gameObject.SetActive(shieldStatus);
        }
    }
}
