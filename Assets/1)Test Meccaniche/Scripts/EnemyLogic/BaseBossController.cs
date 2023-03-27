using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public abstract class BaseBossController : MonoBehaviour
    {

        protected Animator bossAnimator;
        public Transform playerTransform;
        protected HealthComponent bossHealth;

        protected float minAttackCD = 2;
        protected float maxAttackCD = 10;
        protected float attackCD;
        protected float currentAttackCD;

        protected float playerPositionCheckCD = 0.25f;
        protected float currentPlayerPositionCheckCD;

        // Start is called before the first frame update
        protected void Start()
        {
            bossAnimator = GetComponent<Animator>();
            bossHealth = GetComponent<HealthComponent>();
            currentAttackCD = 0;
            currentPlayerPositionCheckCD = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPoiseBreak()
        {
            bossAnimator.SetTrigger("Hit");
        }

        protected void SpawnPowerUp()
        {
            PowerUpsSpawner.SpawnPowerUp(PowerUpType.Permanent).transform.position = transform.position;
        }
    }
}
