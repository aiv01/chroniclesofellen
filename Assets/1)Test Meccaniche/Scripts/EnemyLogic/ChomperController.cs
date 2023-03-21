using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChomperController : BaseEnemyComponent
    {
        [SerializeField]
        private UnityEvent<float> OnPlayerHit;
        public Transform[] patrolPoints;

        private bool isPursuing;
        private bool isPatroling;

        private int currentPatrolPoint;

        private float stayTimer;
        private float currentStayTimer;

        private float pursuingTimer;
        private float currentPursuingTimer;
        private float pursuingDistance;

        //private float attackCD;
        //private float currentAttackCD;

        private float enemyDamage;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            isPursuing = false;
            isPatroling = false;

            stayTimer = enemySO.stayTime;
            currentStayTimer = 0;

            currentPatrolPoint = 0;

            pursuingTimer = enemySO.pursuitTime;
            //currentAttackCD = 0;
            //attackCD = enemySO.attackCD;

            currentPursuingTimer = 0;

            pursuingDistance = enemySO.pursuitDistance * enemySO.pursuitDistance;

            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            agent.speed = enemySO.speed;
            //da vedere come fare per le animazioni
            //enemyAnimator.speed = enemySO.speed;
        }

        // Update is called once per frame
        void Update()
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
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isAttacking && collision.gameObject.tag == "Player")
            {
                OnPlayerHit.Invoke(enemyDamage);
            }
            if (collision.gameObject.tag == "PlayerWeaponHitBox")
            {
                enemyAnimator.SetTrigger("Hit");

            }
        }
        private void ApplyGravity()
        {
            transform.position += new Vector3(0, -9.81f * Time.deltaTime, 0);
        }

        private void Patrol()
        {
            if (!IsPatroling())
            {
                ChangePatrolPoint();
            }
        }
        private bool IsPatroling()
        {
            float distance = (transform.position - patrolPoints[currentPatrolPoint].position).sqrMagnitude;
            isPatroling = distance >= sqrStopDistance;
            enemyAnimator.SetBool("IsPatroling", isPatroling);
            enemyAnimator.SetBool("IsNearBase", !isPatroling);
            return isPatroling;
        }
        private void ChangePatrolPoint()
        {
            currentStayTimer += Time.deltaTime;
            if (currentStayTimer >= stayTimer)
            {
                currentStayTimer = 0;
                currentPatrolPoint = currentPatrolPoint + 1 % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            }
        }

        private void Pursuit()
        {
            if(IsPursuing())
            {
                PursuitPlayer();
            }
        }
        private bool IsPursuing()
        {
            float disance = (transform.position - playerPosition.position).sqrMagnitude;
            isPursuing = disance <= pursuingDistance;
            enemyAnimator.SetBool("IsPursuing", isPursuing);
            return isPursuing;
        }
        private void PursuitPlayer()
        {
            currentPursuingTimer += Time.deltaTime;
            if (currentPursuingTimer >= pursuingTimer)
            {
                agent.SetDestination(playerPosition.position);
            }
        }

        private void Attack()
        {
            if (IsAttacking())
            {
                RotateEnemy();
            }
        }
        private bool IsAttacking()
        {
            float distance = (transform.position - playerPosition.position).sqrMagnitude;
            isAttacking = distance <= attackDistance;
            enemyAnimator.SetBool("IsAttacking", isAttacking);
            return isAttacking;
        }
        private void RotateEnemy()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation((playerPosition.position - transform.position).normalized), Time.deltaTime);
        }

    }

}