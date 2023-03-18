using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen{
public class Staff : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private WeaponType type;
    public WeaponType Type { get {return type;}}
    private int maxUsesSeries = 4 ;
    public int MaxUsesSeries { get {return maxUsesSeries;}}
    
    void Awake()
    {
        type = WeaponType.Staff;
    }

}
}