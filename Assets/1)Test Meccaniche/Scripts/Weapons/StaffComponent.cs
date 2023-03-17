using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen{
public class StaffComponent : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private WeaponType type;
    public WeaponType Type { get {return type;}}
    private int maxUses =10;
    public int MaxUses { get {return maxUses;}}
    
    void Awake()
    {
        type = WeaponType.Staff;
    }

}
}
