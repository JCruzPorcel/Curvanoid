using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerScoreStyles
{
    [Header("Outline")]
    public Color outlineColor;
    public Vector2 outlineEffectDistance;
    [Space(5)][Header("Shadow")]
    public float shadowAlpha;
    public Vector2 shadowEffectDistance;
}

public class ScoreboardController : MonoBehaviour
{
    [Header("Player Score Prefab")]
    [SerializeField] private GameObject playerScorePrefab;
    [SerializeField] private Transform playerScoreContainer;
    [SerializeField] private TextMeshProUGUI emptyListMessageText;
    [SerializeField, TextArea(5, 15)] private string defaultMessageText;

    [Header("Player Score Styles")]
    [SerializeField] private PlayerScoreStyles playerStyles;
    [SerializeField] private PlayerScoreStyles top1Styles;
    [SerializeField] private PlayerScoreStyles top2Styles;
    [SerializeField] private PlayerScoreStyles top3Styles;

    private void Start()
    {
        ShowPlayerData();
    }

    public void ShowPlayerData()
    {
        // Obtener los puntajes de los jugadores
        List<PlayerScoreData> playerScores = DatabaseManager.LoadPlayerScores();

        if (playerScores.Count > 0)
        {
            emptyListMessageText.text = string.Empty;

            // Definir la posición inicial en Y y la altura total requerida
            float initialYPosition = -65f;
            float totalHeight = playerScores.Count * 130f; // Espaciado vertical entre jugadores

            // Ajustar el tamaño del contenedor
            RectTransform containerRectTransform = playerScoreContainer.GetComponent<RectTransform>();
            containerRectTransform.sizeDelta = new Vector2(containerRectTransform.sizeDelta.x, totalHeight);

            // Instanciar un prefab para cada puntaje de jugador
            for (int i = 0; i < playerScores.Count; i++)
            {
                // Instanciar el prefab y configurar su posición en la interfaz
                GameObject playerPrefab = Instantiate(playerScorePrefab, playerScoreContainer);

                // Calcular la posición en Y para este puntaje de jugador
                float yPosition = initialYPosition + (-130f * i);
                playerPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPosition);

                PlayerScoreUI scoreUI = playerPrefab.GetComponent<PlayerScoreUI>();

                // Actualizar el contenido del puntaje de jugador en el prefab
                if (scoreUI != null)
                {
                    scoreUI.UpdatePlayerInfo(playerScores[i], i + 1); // Puesto de jugador (comenzando desde 1)

                    // Estilo del ultimo jugador guardado
                    if (playerScores[i]?.Id == ScoreController.LastPlayerID)
                    {
                        scoreUI.ApplyStyles(playerStyles);
                    }

                    // Aplicar estilos especiales para los tres primeros jugadores
                    if (i == 0)
                    {
                        scoreUI.ApplyStyles(top1Styles);
                    }
                    else if (i == 1)
                    {
                        scoreUI.ApplyStyles(top2Styles);
                    }
                    else if (i == 2)
                    {
                        scoreUI.ApplyStyles(top3Styles);
                    }
                }
            }
        }
        else
        {
            emptyListMessageText.text = defaultMessageText;

            // Restablecer el tamaño del contenedor si no hay jugadores
            RectTransform containerRectTransform = playerScoreContainer.GetComponent<RectTransform>();
            containerRectTransform.sizeDelta = new Vector2(containerRectTransform.sizeDelta.x, 0f);
        }
    }
}
