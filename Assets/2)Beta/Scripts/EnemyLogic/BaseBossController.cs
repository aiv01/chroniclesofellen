using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{

    public abstract class BaseBossController : MonoBehaviour
    {
        [SerializeField]
        protected BossArenaTrigger arenaWalls;
        protected Animator bossAnimator;
        public Transform playerTransform;
        protected HealthComponent bossHealth;
        protected PowerUp pu;

        protected float minAttackCD = 0.5f;
        protected float maxAttackCD = 5f;
        protected float attackCD;
        protected float currentAttackCD;

        protected float playerPositionCheckCD = 0.5f;
        protected float currentPlayerPositionCheckCD;

        // Start is called before the first frame update
        protected void Start()
        {
            bossAnimator = GetComponent<Animator>();
            bossHealth = GetComponent<HealthComponent>();
            currentAttackCD = 0;
            currentPlayerPositionCheckCD = 0;
            pu = PowerUpsSpawner.SpawnPowerUp(PowerUpType.Permanent);
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
