using UnityEngine;

[System.Serializable]
public class PlayerScoreData
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string playerName;

    [SerializeField]
    private int score;

    public PlayerScoreData(int id, string playerName, int score)
    {
        this.id = id;
        this.playerName = playerName;
        this.score = score;
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
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
