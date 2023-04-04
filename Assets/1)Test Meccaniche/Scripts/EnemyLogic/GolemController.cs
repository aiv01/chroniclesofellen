using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace TheChroniclesOfEllen
{

    public class GolemController : BaseBossController
    {
        [SerializeField]
        private EnemyHitBox rightArm;
        [SerializeField]
        private EnemyHitBox leftArm;

        public float closeRangeDistance;
        public float meleeDistance;
        public float walkDistance;
        public float runDistance;
        private int animIsCloseRangeB;
        private int animIsMeleeB;
        private int animIsRunningB;
        private int animIsWalkingB;
        private int animIsWaitingB;
        private int animAngleF;

        private bool isAttacking;
        public bool isMelee;
        public bool isCloseRange;


        // Start is called before the first frame update
        void Start()
        {
            
            base.Start();
            closeRangeDistance *= closeRangeDistance;
            meleeDistance *= meleeDistance;
            walkDistance *= walkDistance;
            runDistance *= runDistance;
            animAngleF = Animator.StringToHash("Angle");
            animIsMeleeB = Animator.StringToHash("IsMelee");
            animIsRunningB = Animator.StringToHash("IsRunning");
            animIsWalkingB = Animator.StringToHash("IsWalking");
            animIsWaitingB = Animator.StringToHash("IsWaiting");
            animIsCloseRangeB = Animator.StringToHash("IsCloseRange");
        }

        // Update is called once per frame
        void Update()
        {
            if (!bossHealth.IsAlive)
            {
                bossAnimator.SetTrigger("Die");
                pu.gameObject.SetActive(true);
                pu.transform.position = transform.position + Vector3.up;
                arenaWalls.gameObject.SetActive(false);
                return;
            }
            currentPlayerPositionCheckCD += Time.deltaTime;
            currentAttackCD += Time.deltaTime;

            if (currentPlayerPositionCheckCD >= playerPositionCheckCD)
            {
                currentPlayerPositionCheckCD = 0;
                Vector3 vDistance = (playerTransform.position - transform.position);
                float distance = vDistance.sqrMagnitude;

                ManageDistance(distance);
                float angle = Vector3.SignedAngle(transform.forward, vDistance, Vector3.up);
                bossAnimator.SetFloat(animAngleF, angle);

            }

            if (isAttacking)
            {
                if (isMelee)
                {
                    rightArm.isAttacking = false;
                    leftArm.isAttacking = true;
                    isAttacking = false;
                }
                else if (isCloseRange)
                {
                    rightArm.isAttacking = true;
                    leftArm.isAttacking = false;
                    isAttacking = false;
                }
                
            }

        }



        private void StartAttack()
        {
            isAttacking = true;
        }

        private void EndAttack()
        {
            isAttacking = false;
            currentAttackCD = 0;
            attackCD = Random.Range(minAttackCD, maxAttackCD);
            bossAnimator.SetBool(animIsWaitingB, true);

        }

        private void ManageDistance(float distance)
        {
            if (distance <= closeRangeDistance) 
            {
                if(currentAttackCD < attackCD)
                {
                    bossAnimator.SetBool(animIsWaitingB, true);
                    return;
                }
                bossAnimator.SetBool(animIsCloseRangeB, true);
                bossAnimator.SetBool(animIsRunningB, false);
                bossAnimator.SetBool(animIsMeleeB, false);
                bossAnimator.SetBool(animIsWalkingB, false);
                bossAnimator.SetBool(animIsWaitingB, false);
            }
            else if (distance <= meleeDistance)
            {
                if(currentAttackCD < attackCD)
                {
                    bossAnimator.SetBool(animIsWaitingB, true);
                    return;
                }
                bossAnimator.SetBool(animIsMeleeB, true);
                bossAnimator.SetBool(animIsCloseRangeB, false);
                bossAnimator.SetBool(animIsRunningB, false);
                bossAnimator.SetBool(animIsWalkingB, false);
                bossAnimator.SetBool(animIsWaitingB, false);
            }
            else if (distance <= walkDistance)
            {
                bossAnimator.SetBool(animIsWalkingB, true);
                bossAnimator.SetBool(animIsCloseRangeB, false);
                bossAnimator.SetBool(animIsRunningB, false);
                bossAnimator.SetBool(animIsMeleeB, false);
                isCloseRange = false;
                isMelee = false;
                transform.forward = Vector3.Lerp(transform.forward,
                    new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z),
                    Time.deltaTime);
            }
            else
            {
                bossAnimator.SetBool(animIsRunningB, true);
                bossAnimator.SetBool(animIsCloseRangeB, false);
                bossAnimator.SetBool(animIsMeleeB, false);
                bossAnimator.SetBool(animIsWalkingB, false);
                isCloseRange = false;
                isMelee = false;
                transform.forward = Vector3.Lerp(transform.forward,
                    new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z),
                    Time.deltaTime);
            }
        }
    }
}