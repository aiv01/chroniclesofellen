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
        [SerializeField]
        private EnemyHitBox biteHitBox;
        private ChomperAudio chomperAudio;

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

        void Awake()
        {
            chomperAudio = GetComponent<ChomperAudio>();
        }
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            isPursuing = false;
            isPatroling = false;

            powerUp = PowerUpsSpawner.SpawnPowerUp(SpawnPowerUp());
            stayTimer = enemySO.stayTime;
            currentStayTimer = 0;

            currentPatrolPoint = 0;

            pursuingTimer = enemySO.pursuitTime;

            biteHitBox.damage = (int)enemySO.damage;
            
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
            if (!enemyHealth.IsAlive)
            {
                if (powerUp != null)
                {
                    powerUp.gameObject.SetActive(true);
                    powerUp.transform.position = transform.position + Vector3.up;
                }
                gameObject.SetActive(false);
                return;
            }
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
            if (collision.gameObject.tag == "Weapon" || collision.gameObject.tag == "PlayerBullet")
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
                biteHitBox.isAttacking = true;
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

        private PowerUpType SpawnPowerUp()
        {
            int powerup = Random.Range(0, 100);

            if (powerup <= 30)
            {
                return PowerUpType.Health;
            }
            else if (powerup <= 50)
            {
                return PowerUpType.Shield;
            }
            else if (powerup <= 55)
            {
                return PowerUpType.Gun;
            }
            else
            {
                return PowerUpType.None;
            }
        }

        public void ReloadChomper()
        {
            enemyHealth.SetMaxHealth((int)enemySO.healthPoint);
            biteHitBox.damage = (int)enemySO.damage;
        }
    }
}