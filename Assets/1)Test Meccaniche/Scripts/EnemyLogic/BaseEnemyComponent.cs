using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


namespace TheChroniclesOfEllen
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HealthComponent))]
    public abstract class BaseEnemyComponent : MonoBehaviour
    {
        public BaseEnemySO enemySO;
        public Transform playerPosition;
        protected NavMeshAgent agent;
        protected Animator enemyAnimator;
        protected HealthComponent enemyHealth;
        [SerializeField]
        protected PowerUp powerUp;

        protected EnemyType type;
        protected bool isAttacking;

        protected float normalStopDistance;
        protected float attackStopDistance;
        protected float sqrStopDistance;
        protected float attackDistance;

        protected void Start()
        {
            isAttacking = false;

            agent = GetComponent<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();
            enemyHealth = GetComponent<HealthComponent>();

            sqrStopDistance = agent.stoppingDistance * agent.stoppingDistance;

            normalStopDistance = enemySO.stopDistance;
            attackStopDistance = enemySO.attackStopDistance;

            attackDistance = enemySO.attackDistance * enemySO.attackDistance;

            enemyHealth.SetMaxHealth(enemySO.healthPoint);

            type = enemySO.type;
        }
    }
}
