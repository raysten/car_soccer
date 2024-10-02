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
        
        private IScoreProvider _scoreProvider;

        [Inject]
        private void Construct(IScoreProvider scoreProvider)
        {
            _scoreProvider = scoreProvider;
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
                
                // @todo: use ScoreProvider

                SpawnBall(runner);
            }
        }
    }
}
