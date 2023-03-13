using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    public partial class BaseEnemyController : MonoBehaviour
    {

        public BaseEnemySO enemySO;
        public Transform tergetPlayer;
        private NavMeshAgent agent;
        private Animator enemyAnimator;

        private EnemyType type;

        private bool isGrounded;
        private bool isAttacking;
        private bool isPursuing;

        private float normalStopDistance;
        private float attackStopDistance;

        private float sqrStopDistance;

        private float pursuingTimer;
        private float currentPursuingTimer;
        private float pursuingDistance;

        private float attackCD;
        private float currentAttackCD;
        private float attackDistance;

        private float enemyHealth;
        private float currentEnemyHealth;

        // Start is called before the first frame update
        void Start()
        {
            isGrounded = true;
            isPursuing = false;
            isAttacking = false;

            normalStopDistance = enemySO.stopDistance;
            attackStopDistance = enemySO.attackStopDistance;

            pursuingTimer = enemySO.pursuitTime;
            attackCD = enemySO.attackCD;
            
            currentPursuingTimer = 0;
            currentAttackCD = 0;

            pursuingDistance = enemySO.pursuitDistance;
            attackDistance = enemySO.attackDistance;

            enemyHealth = enemySO.healthPoint;
            currentEnemyHealth = enemyHealth;
            type = enemySO.type;

            switch (type)
            {
                case EnemyType.Chomper:
                case EnemyType.FastChomper:
                    StartChomper();
                    break;
                case EnemyType.Spitter:
                    StartSpitter();
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: capire come fare sta roba

            if (isGrounded)
            {
                switch (type)
                {
                    case EnemyType.Chomper:
                    case EnemyType.FastChomper:
                        UpdateChomper();
                        break;
                    case EnemyType.Spitter:
                        UpdateSpitter();
                        break;
                }

            }
            else
            {
                ApplyGravity();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 3)
            {
                isGrounded = true;
                enemyAnimator.SetBool("Grounded", true);
            }
            if (collision.gameObject.tag == "PlayerWeaponHitBox")
            {
                enemyAnimator.SetTrigger("Hit");

            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == 3)
            {
                isGrounded = false;
                enemyAnimator.SetBool("Grounded", false);
            }
        }

        private void ApplyGravity()
        {
            transform.position += new Vector3(0, -9.81f * Time.deltaTime, 0);
        }



        private bool IsNearCheckPoint()
        {
            Vector3 deltaPosition = agent.destination - transform.position;

            return deltaPosition.sqrMagnitude <= sqrStopDistance;
        }
        private void MoveToPatroPoint()
        {
            currentStayTimer += Time.deltaTime;

            if (currentStayTimer >= stayTimer)
            {
                currentStayTimer = 0;
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            }
        }

        private void PursuitPlayer()
        {
            currentPursuingTimer += Time.deltaTime;
            if (currentPursuingTimer >= pursuingTimer)
            {
                currentPursuingTimer = 0;
                agent.destination = tergetPlayer.position;
            }
        }
        private bool IsPursuingPlayer()
        {
            float distance = (tergetPlayer.position - transform.position).sqrMagnitude;
            return distance <= pursuingDistance;
        }

        private void AttackPlayer()
        {
            agent.stoppingDistance = attackStopDistance;
            currentAttackCD += Time.deltaTime;
            if (currentAttackCD >= attackCD)
            {
                currentAttackCD = 0;
                enemyAnimator.SetTrigger("Attack");
            }

        }
        private bool IsAttackingPlayer()
        {
            float distance = (tergetPlayer.position - transform.position).sqrMagnitude;
            return distance <= attackDistance;
        }

    }
}