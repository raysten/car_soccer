using System;

namespace Soccer
{
    public interface IBallEvents
    {
        event Action<ETeam> OnBallEnteredGoal;
    }
}
