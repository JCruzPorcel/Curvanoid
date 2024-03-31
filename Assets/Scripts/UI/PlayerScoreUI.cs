using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position_Text;
    [SerializeField] private TextMeshProUGUI name_Text;
    [SerializeField] private TextMeshProUGUI score_Text;

    public void UpdatePlayerInfo(PlayerScoreData data, int pos)
    {
        position_Text.text = pos.ToString();
        name_Text.text = data.PlayerName;
        score_Text.text = data.Score.ToString();
    }
}
