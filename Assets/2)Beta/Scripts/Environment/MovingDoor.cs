using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    [SerializeField]
    private Transform targetOne;
    [SerializeField]
    private Transform targetTwo;
    [SerializeField]
    private Transform doorTransform;
    private float speed;
    private bool isPlayerNear;
    private DoorAudio audio;

    void Awake()
    {
        audio = GetComponent<DoorAudio>();
    }

    void Update()
    {
        if (isPlayerNear)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {

        if (doorTransform.position.y <= targetOne.position.y)
        {
            audio.StopAudio();
            speed = 0;
            return;
        }
        else
        {
            doorTransform.Translate(Vector3.down * speed * Time.deltaTime);
            
        }
    }
    private void Close()
    {
        if (doorTransform.position.y >= targetTwo.position.y)
        {
            audio.StopAudio();
            speed = 0f;
            return;
        }
        else
        {
            doorTransform.Translate(Vector3.up * speed * Time.deltaTime);
            

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerNear = true;
            speed = 2f;
            audio.PlayAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerNear = false;
            speed = 2f;
            audio.PlayAudio();
        }
    }


}
