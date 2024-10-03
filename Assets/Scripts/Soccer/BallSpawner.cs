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
        private NetworkRunner _networkRunner;

        [Inject]
        private void Construct(IScore score, NetworkRunner networkRunner)
        {
            _score = score;
            _networkRunner = networkRunner;
        }

        public void SpawnBall(NetworkRunner runner)
        {
            if (_ballInstance == null)
            {
                _ballInstance = runner.Spawn(_ballPrefab);

                _ballInstance.OnEnteredGoal += GoalScored;
            }

            void GoalScored(ETeam team)
            {
                _ballInstance.OnEnteredGoal -= GoalScored;

                runner.Despawn(_ballInstance.Object);
                _ballInstance = null;
                
                IncrementScore(team);

                SpawnBall(runner);
            }
        }

        private void IncrementScore(ETeam team)
        {
            _score.IncrementScore(team);
        }
    }
}
