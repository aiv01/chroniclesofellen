using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen{
    
public class Key : Interactable
{

    void Update()
    {
        transform.Rotate(Vector3.up,Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnPickUp(other.GetComponent<PlayerController>());
        }
    }

    protected override void OnPickUp(PlayerController player)
    {
        player.HasKey = true;
        Debug.Log("MIO PADRE");
        gameObject.SetActive(false);
    }

}   
}
