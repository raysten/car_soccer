using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Soccer
{
    public class Score : NetworkBehaviour, IScore, IScoreEvents
    {
        public event Action<int, int> OnScoreChanged;

        private NetworkRunner _networkRunner;

        [Networked, OnChangedRender(nameof(NotifyAboutScoreChange))]
        public int RedTeamScore { get; set; }

        [Networked, OnChangedRender(nameof(NotifyAboutScoreChange))]
        public int BlueTeamScore { get; set; }

        [Inject]
        private void Construct(NetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
        }

        public override void Spawned()
        {
            base.Spawned();

            NotifyAboutScoreChange();
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
    }
}
