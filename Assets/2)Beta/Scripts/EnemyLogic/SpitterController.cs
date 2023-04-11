using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ShootComponent))]
    public class SpitterController : BaseEnemyComponent
    {
        private ShootComponent shootComponent;
        [SerializeField]
        private Transform[] fleePoints;

        private bool isFleeing;

        private float fleeingTimer;
        private float currentFleeingTimer;
        private int currentFleePoint;
        [SerializeField]
        private float fleeingDistance;

        private float distanceToFleePoint
        {
            get { return (fleePoints[currentFleePoint].position - transform.position).sqrMagnitude; }
        }

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            isFleeing = false;

            shootComponent = GetComponent<ShootComponent>();
            shootComponent.damage = (int)enemySO.damage;

            fleeingTimer = enemySO.pursuitTime;
            shootComponent.shootCD = enemySO.attackCD;

            currentFleeingTimer = 0;

            fleeingDistance = enemySO.pursuitDistance * enemySO.pursuitDistance;
            currentFleePoint = Random.Range(0, fleePoints.Length);
            agent.SetDestination(fleePoints[currentFleePoint].position);
        }

        // Update is called once per frame
        void Update()
        {
            if (!enemyHealth.IsAlive)
            {
                powerUp = PowerUpsSpawner.SpawnPowerUp(SpawnPowerUp());
                if (powerUp != null)
                {
                    powerUp.gameObject.SetActive(true);
                    powerUp.transform.position = transform.position + Vector3.up;
                }
                gameObject.SetActive(false);
                return;
            }
            Flee();
            if (!isFleeing)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation((playerPosition.position - transform.position).normalized), Time.deltaTime);
                Attack();
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

        private void Flee()
        {
            if (IsFleeing())
            {
                RunAway();
                agent.isStopped = false;
            }
            else
            {
                agent.isStopped = true;
            }
        }
        private bool IsFleeing()
        {
            float distance = (transform.position - playerPosition.position).sqrMagnitude;
            isFleeing = distance < fleeingDistance;
            enemyAnimator.SetBool("IsFleeing", isFleeing);
            return isFleeing;
        }
        private void RunAway()
        {
            if (distanceToFleePoint <= sqrStopDistance)
            {
                currentFleePoint = Random.Range(0, fleePoints.Length);
                agent.SetDestination(fleePoints[currentFleePoint].position);
            }
        }

        private void Attack()
        {
            IsShooting();
        }
        private void IsShooting()
        {
            float distance = (transform.position - playerPosition.position).sqrMagnitude;
            isAttacking = distance <= attackDistance;
            enemyAnimator.SetBool("IsAttacking", isAttacking);
        }

        private void Shoot()
        {
            shootComponent.OnShoot(playerPosition.position);
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
                return PowerUpType.Gun;
            }
            else if (powerup <= 55)
            {
                return PowerUpType.Shield;
            }
            else
            {
                return PowerUpType.None;
            }
        }
        public void ReloadSpitter()
        {

            Start();
            gameObject.SetActive(true);
        }
    }
}
