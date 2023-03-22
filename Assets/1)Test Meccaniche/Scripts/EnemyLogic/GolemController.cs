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
            Vector3 angle = Quaternion.FromToRotation(transform.forward, (transform.position - playerTransform.position).normalized).eulerAngles;
            float x = Vector3.Dot(angle.normalized, transform.forward);
            golemAnimator.SetFloat("Angle", x);
        }
    }

}