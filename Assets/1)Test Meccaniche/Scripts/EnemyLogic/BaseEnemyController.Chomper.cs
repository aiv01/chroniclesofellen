using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public partial class BaseEnemyController : MonoBehaviour
    {

        public Transform[] patrolPoints;

        private bool isPatroling;
        private int currentPatrolPoint;

        private float stayTimer;
        private float currentStayTimer;

        // Start is called before the first frame update
        private void StartChomper()
        {
            isPatroling = false;

            stayTimer = enemySO.stayTime;
            currentStayTimer = 0;

            currentPatrolPoint = 0;
        }

        private void UpdateChomper()
        {
            Attack();
            if (!isAttacking)
            {
                agent.stoppingDistance = normalStopDistance;
                PursuitChomper();
                if (!isPursuing)
                {
                    PatrolChomper();
                }
            }
        }

        private void PatrolChomper()
        {
            isPatroling = IsNearCheckPoint();
            enemyAnimator.SetBool("NearBase", isPatroling);
            if (isPatroling)
            {
                MoveToPatroPoint();
            }
        }
        private void PursuitChomper()
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
    }

}