using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{

    public class GolemController : MonoBehaviour
    {
        
        private Animator golemAnimator;
        public Transform playerTransform;
        [SerializeField]
        private UnityEvent OnDie;
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

        public float maxHP;
        public float currHP;

        private bool isAttacking;
        public bool isMelee;
        public bool isCloseRange;

        private float playerPositionCheckCD = 0.25f;
        private float currentPlayerPositionCheckCD;
        private float minAttackCD = 2;
        private float maxAttackCD = 10;
        private float attackCD;
        private float currentAttackCD;

        // Start is called before the first frame update
        void Start()
        {
            golemAnimator = GetComponent<Animator>();

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
            currentPlayerPositionCheckCD = 0;
            currentAttackCD = 0;
        }

        // Update is called once per frame
        void Update()
        {
            currentPlayerPositionCheckCD += Time.deltaTime;
            currentAttackCD += Time.deltaTime;

            Vector3 vDistance = (playerTransform.position - transform.position);

            if (currentPlayerPositionCheckCD >= playerPositionCheckCD)
            {
                float distance = vDistance.sqrMagnitude;

                ManageDistance(distance);
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

            float angle = Vector3.SignedAngle(transform.forward, vDistance, Vector3.up);

            golemAnimator.SetFloat(animAngleF, angle);
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
            Debug.Log(attackCD);

        }

        private void ManageDistance(float distance)
        {
            if (distance <= closeRangeDistance) 
            {
                if(currentAttackCD < attackCD)
                {
                    golemAnimator.SetBool(animIsWaitingB, true);
                    return;
                }
                golemAnimator.SetBool(animIsCloseRangeB, true);
                golemAnimator.SetBool(animIsRunningB, false);
                golemAnimator.SetBool(animIsMeleeB, false);
                golemAnimator.SetBool(animIsWalkingB, false);
                golemAnimator.SetBool(animIsWaitingB, false);
            }
            else if (distance <= meleeDistance)
            {
                if(currentAttackCD < attackCD)
                {
                    golemAnimator.SetBool(animIsWaitingB, true);
                    return;
                }
                golemAnimator.SetBool(animIsMeleeB, true);
                golemAnimator.SetBool(animIsCloseRangeB, false);
                golemAnimator.SetBool(animIsRunningB, false);
                golemAnimator.SetBool(animIsWalkingB, false);
                golemAnimator.SetBool(animIsWaitingB, false);
            }
            else if (distance <= walkDistance)
            {
                golemAnimator.SetBool(animIsWalkingB, true);
                golemAnimator.SetBool(animIsCloseRangeB, false);
                golemAnimator.SetBool(animIsRunningB, false);
                golemAnimator.SetBool(animIsMeleeB, false);
                transform.forward = Vector3.Lerp(transform.forward,
                    new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z),
                    Time.deltaTime);
            }
            else
            {
                golemAnimator.SetBool(animIsRunningB, true);
                golemAnimator.SetBool(animIsCloseRangeB, false);
                golemAnimator.SetBool(animIsMeleeB, false);
                golemAnimator.SetBool(animIsWalkingB, false);
                transform.forward = Vector3.Lerp(transform.forward,
                    new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z),
                    Time.deltaTime);
            }
        }
    }
}