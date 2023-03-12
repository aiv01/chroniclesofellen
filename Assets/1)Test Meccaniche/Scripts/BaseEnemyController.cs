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
        private int currentPatrolPoint;
        private float sqrStopDistance;
        [SerializeField]
        private float stayTimer = 2;
        private float currentStayTimer;
        private float pursuingTimer = 0.5f;
        private float currentPursuingTimer;

        private NavMeshAgent agent;
        private Animator enemyAnimator;
        public Transform[] patrolPoints;
        public Transform tergetPursuing;



        // Start is called before the first frame update
        void Start()
        {
            enemyAnimator = GetComponent<Animator>();
            isGrounded = true;
            enemyAnimator.SetBool("Grounded", isGrounded);
            enemyAnimator.SetBool("NearBase", false);
            currentPatrolPoint = 0;
            currentStayTimer = 0;
            currentPursuingTimer = 0;
            agent = GetComponent<NavMeshAgent>();
            sqrStopDistance = agent.stoppingDistance * agent.stoppingDistance;
            agent.SetDestination(patrolPoints[0].position);
        }

        // Update is called once per frame
        void Update()
        {
            if (isGrounded)
            {
                Patrol();
                Pursuit();
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
            bool nearBase = IsNearCheckPoint();
            enemyAnimator.SetBool("NearBase", nearBase);

            if (nearBase)
            {
                MoveToPatroPoint();
            }
        }
        private void Pursuit()
        {
            bool inPursuit = IsPursuingPlayer();
            enemyAnimator.SetBool("InPursuit", inPursuit);

            if (inPursuit)
            {
                PursuitPlayer();
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
                agent.destination = tergetPursuing.position;
            }
        }
        private bool IsPursuingPlayer()
        {
            if (currentPatrolPoint >= 2)
            {
                return true;
            }
            return false;
        }
    }
}