using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen{
public class StaffPedestal : MonoBehaviour
{
    private float angle;

    private GameObject staffTransform;

    void Start()
    {
        staffTransform = transform.Find("Staff").gameObject;

    }
    private void Update()
    {
        angle += 4f * Time.deltaTime;
        staffTransform.gameObject.transform.localPosition = Vector3.forward * Mathf.Sin(angle) * 0.5f;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            staffTransform.SetActive(false);
        }
    }
}
}
