using Fusion;
using UnityEngine;

namespace Ball
{
    public class BallSpawner : MonoBehaviour, IBallSpawner
    {
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

            void GoalScored()
            {
                _ballInstance.OnEnteredGoal -= GoalScored;
                
                runner.Despawn(_ballInstance.Object);
                _ballInstance = null;

                SpawnBall(runner);
            }
        }
    }
}
