using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    public class BaseEnemyController : MonoBehaviour
    {
        private bool isGrounded;
        private bool isAttacking;
        private bool isPatroling;
        private bool isPursuing;
        private int currentPatrolPoint;
        private float sqrStopDistance;
        [SerializeField]
        private float stayTimer;
        private float currentStayTimer;
        [SerializeField]
        private float pursuingTimer;
        private float currentPursuingTimer;
        [SerializeField]
        private float attackCD;
        private float currentAttackCD;
        public float attackDistance;
        public float pursuingDistance;

        private NavMeshAgent agent;
        private Animator enemyAnimator;
        public Transform[] patrolPoints;
        public Transform tergetPlayer;



        // Start is called before the first frame update
        void Start()
        {
            enemyAnimator = GetComponent<Animator>();
            isGrounded = true;
            isAttacking = false;
            isPatroling = false;
            isPursuing = false;
            enemyAnimator.SetBool("Grounded", isGrounded);
            enemyAnimator.SetBool("NearBase", false);
            currentPatrolPoint = 0;
            currentStayTimer = 0;
            currentPursuingTimer = 0;
            currentAttackCD = 0;
            agent = GetComponent<NavMeshAgent>();
            sqrStopDistance = agent.stoppingDistance * agent.stoppingDistance;
            attackDistance = attackDistance * attackDistance;
            pursuingDistance = pursuingDistance * pursuingDistance;
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
                    Pursuit();
                    if (!isPursuing)
                    {
                        Patrol();
                    }
                }
                if (!isAttacking && !isPursuing)
                {
                    Patrol();
                }
                if (isPursuing && !isPatroling)
                {
                    Attack();
                }
                if (!isAttacking)
                {
                    Pursuit();
                }

            }
            else
            {
                ApplyGravity();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("dio madonna");
            if (collision.gameObject.layer == 3)
            {
                isGrounded = true;
                enemyAnimator.SetBool("Grounded", true);
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("dio madonna");
            if (collision.gameObject.layer == 3)
            {
                isGrounded = false;
                enemyAnimator.SetBool("Grounded", false);
            }
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
            isPatroling = isPursuing;
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

        private void ApplyGravity()
        {
            transform.position += new Vector3(0, -9.81f * Time.deltaTime, 0);
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