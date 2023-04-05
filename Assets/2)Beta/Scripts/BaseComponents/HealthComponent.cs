using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace TheChroniclesOfEllen
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnPoiseBreak;
        public UIHealthBar healthBar;
        [SerializeField]
        private int maxHealth;
        public int currentHealth;
        private bool shieldActive;
        [SerializeField]
        private float maxPoise;
        private float currPoise;
        [SerializeField]
        private float regenPoisePerSec;
        private PlayerPowerUp powerUpSystem;
        public Transform shield;
        public Image shieldUI;
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
            if (tag == "Player")
            {
                shield.gameObject.SetActive(false);
                healthBar.LoadHealth((int)maxHealth);
                healthBar.ChangeHealth((int)currentHealth);
            }
            isAlive = true; 
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
                bool shieldStatus = powerUpSystem.IsShieldActive();
                shield.gameObject.SetActive(shieldStatus);
                shieldUI.gameObject.SetActive(shieldStatus);

                healthBar.LoadHealth((int)maxHealth);
                healthBar.ChangeHealth((int)currentHealth);
            }
            if (currentHealth <= 0)
            {
                isAlive = false;
                currentHealth = 0;
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
        }
        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void HealMe(int healAmount)
        {
            currentHealth =(int)MathF.Min(healAmount + currentHealth, maxHealth);
        }

        public void TakeDamage(int damageAmount)
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
                OnPoiseBreak.Invoke();
                currPoise = maxPoise;
            }

            currentHealth -= damageAmount;
        }

        public void IncreaseMaxHealth(int healthIncreaseValue)
        {
            if (tag == "Player")
            {
                maxHealth += healthIncreaseValue;
                maxHealth = Mathf.Min(maxHealth, 10);
                currentHealth = maxHealth;
            }
        }

        public void Shield(bool shieldStatus)
        {
            shield.gameObject.SetActive(shieldStatus);
        }
    }
}
