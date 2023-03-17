using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen{
public abstract class Interactable : MonoBehaviour
{
    protected virtual void OnPickUp(PlayerController player) {}
    
}
}
