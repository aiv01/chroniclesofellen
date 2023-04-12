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
        public bool HasKey;
        public BossStatus GolemStatus;
        public BossStatus MotherSpitterStatus;
        public Area Area;
        public int SavePointNumber;
    }

}