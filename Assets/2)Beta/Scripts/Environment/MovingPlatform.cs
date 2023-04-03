using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheChroniclesOfEllen
{

    public class MovingPlatform : MonoBehaviour
    {
        public Transform start;
        public Transform end;

        public float platformSpeed;
        public float platformWaitTimer;
        private float currPlatformWaitTimer;
        public float stopDistance = 1;

        // Start is called before the first frame update
        void Start()
        {
            currPlatformWaitTimer = 0;
            stopDistance *= stopDistance;
        }

        // Update is called once per frame
        void Update()
        {
            MoveToEnd();
        }

        private void MoveToEnd()
        {
            Vector3 distance = end.position - transform.position; 
            if (distance.sqrMagnitude <= stopDistance)
            {
                currPlatformWaitTimer += Time.deltaTime;

                if(currPlatformWaitTimer >= platformWaitTimer)
                {
                    currPlatformWaitTimer = 0;
                    ChangeDestination();
                }
            }
            else
            {
                transform.position += distance.normalized * platformSpeed * Time.deltaTime;
            }

            
        }

        private void ChangeDestination()
        {
            Transform temp = start;
            start = end;
            end = temp;
        }
    }

}