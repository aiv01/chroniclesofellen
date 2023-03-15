using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public abstract class PowerUpSO : ScriptableObject
    {
        protected PowerUpType type;

        public abstract void SetPowerUpType();
    }
}

