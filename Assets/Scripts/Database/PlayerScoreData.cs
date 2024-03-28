[System.Serializable]
public class PlayerScoreData
{
    private string playerName;
    private int score;

    public PlayerScoreData(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }
}
