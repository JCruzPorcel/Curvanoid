using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI position_Text;
    [SerializeField] private TextMeshProUGUI name_Text;
    [SerializeField] private TextMeshProUGUI score_Text;

    [Header("Outline")]
    [SerializeField] private Outline position_Outline;
    [SerializeField] private Outline name_Outline;
    [SerializeField] private Outline score_Outline;

    [Header("Shadow")]
    [SerializeField] private Shadow position_Shadow;
    [SerializeField] private Shadow name_Shadow;
    [SerializeField] private Shadow score_Shadow;

    public void UpdatePlayerInfo(PlayerScoreData data, int pos)
    {
        // Actualizar los textos
        position_Text.text = pos.ToString();
        name_Text.text = data.PlayerName;
        score_Text.text = data.Score.ToString();
    }

    public void ApplyStyles(PlayerScoreStyles styles)
    {
        ApplyOutlineStyles(styles);
        ApplyShadowStyles(styles);
    }

    private void ApplyOutlineStyles(PlayerScoreStyles styles)
    {
        ApplyOutlineStyle(position_Outline, styles.outlineEffectDistance, styles.outlineColor, styles.outlineColor.a);
        ApplyOutlineStyle(name_Outline, styles.outlineEffectDistance, styles.outlineColor, styles.outlineColor.a);
        ApplyOutlineStyle(score_Outline, styles.outlineEffectDistance, styles.outlineColor, styles.outlineColor.a);
    }

    private void ApplyOutlineStyle(Outline outline, Vector2 effectDistance, Color color, float alpha)
    {
        if (outline != null)
        {
            outline.effectDistance = effectDistance;
            outline.effectColor = new Color(color.r, color.g, color.b, alpha);
        }
    }

    private void ApplyShadowStyles(PlayerScoreStyles styles)
    {
        ApplyShadowStyle(position_Shadow, styles.shadowEffectDistance, styles.shadowAlpha);
        ApplyShadowStyle(name_Shadow, styles.shadowEffectDistance, styles.shadowAlpha);
        ApplyShadowStyle(score_Shadow, styles.shadowEffectDistance, styles.shadowAlpha);
    }

    private void ApplyShadowStyle(Shadow shadow, Vector2 effectDistance, float alpha)
    {
        if (shadow != null)
        {
            shadow.effectDistance = effectDistance;
            shadow.effectColor = new Color(shadow.effectColor.r, shadow.effectColor.g, shadow.effectColor.b, alpha);
        }
    }
}
