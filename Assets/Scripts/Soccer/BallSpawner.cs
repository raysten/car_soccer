using Fusion;
using UnityEngine;
using Zenject;

namespace Soccer
{
    public class BallSpawner : MonoBehaviour, IBallSpawner
    {
        [SerializeField]
        private SoccerBall _ballPrefab;

        private SoccerBall _ballInstance;
        
        private IScore _score;

        [Inject]
        private void Construct(IScore score)
        {
            _score = score;
        }

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
                
                IncrementScore(goalTeam);

                SpawnBall(runner);
            }
        }

        private void IncrementScore(ETeam goalTeam)
        {
            _score.IncrementScore(goalTeam);
        }
    }
}
