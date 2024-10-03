namespace Soccer
{
    public interface IScore
    {
        int RedTeamScore { get; }
        int BlueTeamScore { get; }
        void IncrementScore(ETeam team);
    }
}
