using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Soccer
{
    public class Score : NetworkBehaviour, IScoreEvents
    {
        public event Action<int, int> OnScoreChanged;

        private NetworkRunner _networkRunner;
        private IBallEvents _ballEvents;

        [Networked, OnChangedRender(nameof(NotifyAboutScoreChange))]
        public int RedTeamScore { get; set; }

        [Networked, OnChangedRender(nameof(NotifyAboutScoreChange))]
        public int BlueTeamScore { get; set; }

        [Inject]
        private void Construct(NetworkRunner networkRunner, IBallEvents ballEvents)
        {
            _networkRunner = networkRunner;
            _ballEvents = ballEvents;
        }

        public override void Spawned()
        {
            base.Spawned();

            NotifyAboutScoreChange();
            _ballEvents.OnBallEnteredGoal += IncrementScore;
        }

        private void NotifyAboutScoreChange()
        {
            OnScoreChanged?.Invoke(RedTeamScore, BlueTeamScore);
        }

        public void IncrementScore(ETeam goalTeam)
        {
            if (_networkRunner.IsServer)
            {
                switch (goalTeam)
                {
                    case ETeam.Red:
                        BlueTeamScore++;
                        break;
                    case ETeam.Blue:
                        RedTeamScore++;
                        break;
                    default:
                        Debug.LogError($"Unsupported team {goalTeam}");
                        break;
                }
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            
            _ballEvents.OnBallEnteredGoal -= IncrementScore;
        }
    }
}
