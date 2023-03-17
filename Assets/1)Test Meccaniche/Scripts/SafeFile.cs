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
        public bool HasDoubleJump;
        public bool HasKey;
        public bool HasDash;
        public int Progression;
        public int Area;
        public int SavePointNumber;

        public override string ToString()
        {
            string s = "MaxHp: " + MaxHp +
                "\nDamageScale: " + DamageScale + "" +
                "\nJump: " + HasDoubleJump +
                "\nKey: " + HasKey +
                "\nProgression: " + Progression +
                "\nArea: " + Area +
                "\nsavepoint: " + SavePointNumber;
            return s;
        }
    }
}
