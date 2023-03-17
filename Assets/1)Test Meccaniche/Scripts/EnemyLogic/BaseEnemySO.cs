using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    [CreateAssetMenu(menuName = "Window/Base Enemy", fileName = "Enemy")]
    public class BaseEnemySO : ScriptableObject
    {
        public float healthPoint;
        public float damage;
        public float speed;
        public EnemyType type;

        public float stayTime, attackCD, pursuitTime;
        public float stopDistance, attackStopDistance;
        public float attackDistance, pursuitDistance;

        //public Transform[] patrolPoints;

    }
}

