using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace TheChroniclesOfEllen
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseEnemyComponent : MonoBehaviour
    {
        public BaseEnemySO enemySO;
        public Transform playerPosition;
        protected NavMeshAgent agent;
        protected Animator enemyAnimator;

        protected EnemyType type;
        protected bool isAttacking;

        protected float normalStopDistance;
        protected float attackStopDistance;
        protected float sqrStopDistance;
        protected float attackDistance;
        
        protected float enemyHealth;
        protected float currentEnemyHealth;

        protected void Start()
        {
            isAttacking = false;

            agent = GetComponent<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();

            sqrStopDistance = agent.stoppingDistance * agent.stoppingDistance;

            normalStopDistance = enemySO.stopDistance;
            attackStopDistance = enemySO.attackStopDistance;

            attackDistance = enemySO.attackDistance * enemySO.attackDistance;

            enemyHealth = enemySO.healthPoint;
            currentEnemyHealth = enemyHealth;
            type = enemySO.type;
        }
    }
}
