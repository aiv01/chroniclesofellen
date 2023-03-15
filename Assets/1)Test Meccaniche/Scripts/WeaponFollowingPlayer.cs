using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFollowingPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform playerTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTarget.position;
    }
}
