using System;
using Fusion;
using UnityEngine;

namespace Soccer
{
    public class BallSpawner : MonoBehaviour, IBallSpawner, IBallEvents
    {
        public event Action<ETeam> OnBallEnteredGoal;

        [SerializeField]
        private SoccerBall _ballPrefab;

        private SoccerBall _ballInstance;

        public void SpawnBall(NetworkRunner runner)
        {
            if (_ballInstance == null)
            {
                _ballInstance = runner.Spawn(_ballPrefab);

                _ballInstance.OnEnteredGoal += GoalScored;
            }

            void GoalScored(ETeam goalTeam)
            {
                _ballInstance.OnEnteredGoal -= GoalScored;

                runner.Despawn(_ballInstance.Object);
                _ballInstance = null;

                OnBallEnteredGoal?.Invoke(goalTeam);

                SpawnBall(runner);
            }
        }
    }
}
