using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public enum Area
    {
        Ship,
        Temple1,
        Temple2,
        ChomperNest
    }

    public enum BossStatus
    {
        Disable,
        Active,
        Dead
    }

    public enum EnemyType
    {
        Chomper,
        FastChomper,
        Spitter,
        Golem,
        BigChomper,
        GolemBoss,
        GunGolem
    }

    public enum WeaponType
    {
        None,
        Pistol,
        Staff,
        FinalWeapon
    }

    public enum PowerUpType
    {
        Shield,
        Gun,
        Health,
        Permanent,
        None
    }
    public enum InteractableType
    {
        StaffPedestal,
        SavePoint,
        Key,
        None
    }
}
