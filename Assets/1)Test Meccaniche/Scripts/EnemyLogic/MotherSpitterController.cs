using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class MotherSpitterController : BaseBossController
    {
        private ShootComponent spitterShootComponent;
        [SerializeField]
        private EnemyHitBox biteHitBox;
        public Transform[] fleePoints;
        private int currFleePoint;

        public float shootDistance;
        public float fleeDistance;
        public float fleeStopDistance;
        public float meleeDistance;
        public float meleeStopDistance;

        private int animIsShootingB;
        private int animIsFleeingB;
        private int animIsAttackingB;

        private bool isFleeing;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            shootDistance *= shootDistance;
            fleeDistance *= fleeDistance;
            meleeDistance *= meleeDistance;
            spitterShootComponent = GetComponent<ShootComponent>();

            animIsAttackingB = Animator.StringToHash("IsAttacking");
            animIsShootingB = Animator.StringToHash("IsShooting");
            animIsFleeingB = Animator.StringToHash("IsFleeing");
            isFleeing = false;
        }

        // Update is called once per frame
        void Update()
        {
            currentPlayerPositionCheckCD += Time.deltaTime;
            currentAttackCD += Time.deltaTime;


            if (isFleeing)
            {
                isFleeing = (transform.position - fleePoints[currFleePoint].position).sqrMagnitude <= fleeStopDistance;
            }

            if (!isFleeing && currentPlayerPositionCheckCD >= playerPositionCheckCD)
            {
                currentPlayerPositionCheckCD = 0;
                float distance = (playerTransform.position - transform.position).sqrMagnitude;

                ManageDistance(distance);

                transform.forward = Vector3.Lerp(transform.forward,
                    new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z),
                    Time.deltaTime);
            }

        }

        private void ManageDistance(float distance)
        {
            if(distance<=fleeDistance)
            {
                bossAnimator.SetBool(animIsShootingB, false);
                if (bossHealth.HealthPerc <= 40 && distance <= meleeDistance)
                {
                    if(Random.RandomRange(0,100)<=0.5f*(1- bossHealth.HealthPerc))
                    {
                        bossAnimator.SetBool(animIsAttackingB, true);
                        bossAnimator.SetBool(animIsFleeingB, false);
                        biteHitBox.isAttacking = true;
                        return;
                    }

                }
                Flee();
                bossAnimator.SetBool(animIsFleeingB, true);
                bossAnimator.SetBool(animIsAttackingB, false);
            }
            else if (distance <= shootDistance)
            {
                if(currentAttackCD>= attackCD)
                {
                    bossAnimator.SetBool(animIsShootingB, true);
                    bossAnimator.SetBool(animIsAttackingB, false);
                    bossAnimator.SetBool(animIsFleeingB, false);
                }
            }
        }

        private void Flee()
        {
            isFleeing = true;
            currFleePoint = Random.Range(0, fleePoints.Length);
        }

        public void Shoot()
        {
            spitterShootComponent.OnShoot((playerTransform.position - transform.position).normalized);
        }
    }

}