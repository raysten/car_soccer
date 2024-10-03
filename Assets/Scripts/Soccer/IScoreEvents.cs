using System;

namespace Soccer
{
    public interface IScoreEvents
    {
        event Action<int, int> OnScoreChanged;
    }
}
