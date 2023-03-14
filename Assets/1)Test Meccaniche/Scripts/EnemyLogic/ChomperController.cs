using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;
using UnityEngine.AI;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChomperController : MonoBehaviour
    {
        public BaseEnemySO enemySO;
        public Transform tergetPlayer;
        private NavMeshAgent agent;
        private Animator enemyAnimator;
        public Transform[] patrolPoints;

        private EnemyType type;

        private bool isGrounded;
        private bool isAttacking;
        private bool isPursuing;
        private bool isPatroling;

        private float normalStopDistance;
        private float attackStopDistance;

        private int currentPatrolPoint;
        private float sqrStopDistance;

        private float stayTimer;
        private float currentStayTimer;

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
            isPatroling = false;

            stayTimer = enemySO.stayTime;
            currentStayTimer = 0;

            currentPatrolPoint = 0;

            agent = GetComponent<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();

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
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: capire come fare sta roba

            if (isGrounded)
            {
                Attack();
                if (!isAttacking)
                {
                    agent.stoppingDistance = normalStopDistance;
                    Pursuit();
                    if (!isPursuing)
                    {
                        Patrol();
                    }
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

        private void Patrol()
        {
            isPatroling = IsNearCheckPoint();
            enemyAnimator.SetBool("NearBase", isPatroling);
            if (isPatroling)
            {
                MoveToPatroPoint();
            }
        }
        private void Pursuit()
        {
            isPursuing = IsPursuingPlayer();
            enemyAnimator.SetBool("InPursuit", isPursuing);
            if (isPursuing)
            {
                PursuitPlayer();
            }
        }
        private void Attack()
        {
            isAttacking = IsAttackingPlayer();
            enemyAnimator.SetBool("InPursuit", !isAttacking);
            isPursuing = !isAttacking;
            if (isAttacking)
            {
                AttackPlayer();
            }
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
            return distance <= (attackDistance * attackDistance);
        }
    }

}