using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ShootComponent))]
    public class SpitterController : BaseEnemyComponent
    {
        public Transform fleeDirection;
        private ShootComponent shootComponent;
        [SerializeField]
        private Transform[] fleePoints;

        private bool isFleeing;

        private float fleeingTimer;
        private float currentFleeingTimer;
        private int currentFleePoint;

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
            Flee();
            if (!isFleeing)
            {
                Attack();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "PlayerWeaponHitBox")
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
            }
            else
                agent.Stop();
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
            if (IsShooting())
            {

            }
        }
        private bool IsShooting()
        {
            float distance = (transform.position - playerPosition.position).sqrMagnitude;
            isAttacking = distance <= attackDistance;
            enemyAnimator.SetBool("IsAttacking", isAttacking);
            return isAttacking;
        }
        private void Shoot()
        {
            //usare in qualche modo lo sparo
        }

    }
}
