using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    [System.Serializable]
    public class SafeFile
    {
        public int MaxHp;
        public int DamageScale;
        public bool HasKey;
        public BossStatus GolemStatus;
        public BossStatus MotherSpitterStatus;
        public Area Area;
        public int SavePointNumber;

        public override string ToString()
        {
            string s = "MaxHp: " + MaxHp +
                "\nDamageScale: " + DamageScale + "" +
                "\nKey: " + HasKey +
                "\nGolemStatus: " + GolemStatus +
                "\nMotherSpitterStatus: " + MotherSpitterStatus +
                "\nArea: " + Area +
                "\nsavepoint: " + SavePointNumber;
            return s;
        }
    }
}
