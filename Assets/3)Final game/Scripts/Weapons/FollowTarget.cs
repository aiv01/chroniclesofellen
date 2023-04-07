using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(0,1.5f,-1.5f);
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
}
