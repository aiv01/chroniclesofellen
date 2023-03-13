using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public partial class BaseEnemyController : MonoBehaviour
    {
        
        private void StartSpitter()
        {
        }

        private void UpdateSpitter()
        {

        }

        private void SpotSpitter()
        {
            isAttacking = IsAttackingPlayer();
            enemyAnimator.SetBool("HaveTarget", isAttacking);

            if (isAttacking)
            {

            }
        }

        private void AttackSpitter()
        {
            //TODO: usare gli eventi
        }
    }
}