using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    [CreateAssetMenu(menuName = "Window/DefaultSafeFile", fileName = "DefaultSafeFile")]
    public class SafeFileSO : ScriptableObject
    {

        public int MaxHp;
        public int DamageScale;
        public bool HasDoubleJump;
        public bool HasKey;
        public bool HasDash;
        public int Progression;
        public int Area;
        public int SavePointNumber;
    }

}