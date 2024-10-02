using System;
using Fusion;
using UnityEngine;

namespace Soccer
{
    public class SoccerBall : NetworkBehaviour
    {
        public event Action<ETeam> OnEnteredGoal;
    
        private void OnTriggerStay(Collider otherCollider)
        {
            if (HasStateAuthority && otherCollider.gameObject.layer == Constants.GOAL_LAYER)
            {
                var goal = otherCollider.GetComponent<Goal>();
                OnEnteredGoal?.Invoke(goal.Team);
            }
        }
    }
}
