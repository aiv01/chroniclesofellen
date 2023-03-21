using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    
    public class GolemController : MonoBehaviour
    {
        private Animator golemAnimator;
        public Transform playerTransform;
        // Start is called before the first frame update
        void Start()
        {
            golemAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                golemAnimator.SetTrigger("IsDead");
            }

            float distance = (transform.position - playerTransform.position).magnitude;

            if (distance <= 20)
            {
                golemAnimator.SetFloat("PlayerDistance", distance); 
            }
            float angle = Vector3.Dot(transform.position, playerTransform.position) / 360;
            golemAnimator.SetFloat("Angle", angle);
        }
    }

}