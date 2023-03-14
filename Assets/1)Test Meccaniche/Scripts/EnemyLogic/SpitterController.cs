using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ShootComponent))]
    public class SpitterController : MonoBehaviour
    {
        public BaseEnemySO enemySO;
        public Transform fleeDirection;
        public Transform playerPosition;
        private NavMeshAgent agent;
        private Animator enemyAnimator;
        private ShootComponent shootComponent;

        private EnemyType type;

        private bool isGrounded;
        private bool isAttacking;
        private bool isFleeing;

        private float normalStopDistance;
        private float attackStopDistance;
        private float sqrStopDistance;

        private float fleeingTimer;
        private float currentFleeingTimer;

        private float fleeingDistance;
        private float attackDistance;

        private float enemyHealth;
        private float currentEnemyHealth;




        // Start is called before the first frame update
        void Start()
        {
            isGrounded = true;
            isFleeing = false;
            isAttacking = false;

            agent = GetComponent<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();
            shootComponent = GetComponent<ShootComponent>();

            normalStopDistance = enemySO.stopDistance;
            attackStopDistance = enemySO.attackStopDistance;

            fleeingTimer = enemySO.pursuitTime; 
            shootComponent.shootCD = enemySO.attackCD;

            currentFleeingTimer = 0;

            fleeingDistance = enemySO.pursuitDistance * enemySO.pursuitDistance;
            attackDistance = enemySO.attackDistance * enemySO.attackDistance;

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
                Flee();
                if (!isFleeing)
                {
                    Attack();
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

        private void Flee()
        {
            isFleeing = IsFleeingPlayer();
            enemyAnimator.SetBool("Fleeing", isFleeing);
            if (isFleeing)
            {
                FleePlayer();
            }
        }
        private void FleePlayer()
        {
            currentFleeingTimer += Time.deltaTime;
            if (currentFleeingTimer >= fleeingTimer)
            {
                currentFleeingTimer = 0;
                agent.destination = fleeDirection.position;
            }
        }
        private bool IsFleeingPlayer()
        {
            float distance = (playerPosition.position - transform.position).sqrMagnitude;
            return distance <= fleeingDistance;
        }

        private void Attack()
        {
            bool haveTarget = IsAttackingPlayer();
            enemyAnimator.SetBool("HaveTarget", haveTarget);
            if (haveTarget)
            {
                enemyAnimator.SetTrigger("Attack");
                AttackPlayer();
            }

        }
        private void AttackPlayer()
        {
            shootComponent.Shoot();
        }
        private bool IsAttackingPlayer()
        {
            float distance = (transform.position - playerPosition.position).sqrMagnitude;
            return distance <= attackDistance;
        }
    }
}
