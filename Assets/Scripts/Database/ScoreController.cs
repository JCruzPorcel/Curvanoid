using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static int? LastPlayerID { get; set; } = null;
    public int score { get; private set; } = 0;

    public TMP_InputField NameInputField { get; set; }
    [SerializeField] private TextMeshProUGUI scoreText;

    public bool SaveScore()
    {
        // Obtener el nombre del jugador del campo de entrada
        string playerName = NameInputField.text;

        // Verificar si el nombre del jugador es v�lido
        if (IsValidPlayerName(playerName))
        {
            // Generar un ID �nico para el jugador
            int playerId = GenerateUniquePlayerId();

            LastPlayerID = playerId;

            // Crear un objeto PlayerScoreData con el ID, nombre y puntaje
            PlayerScoreData playerScore = new PlayerScoreData(playerId, playerName, score);

            try
            {
                // Intentar guardar el puntaje
                DatabaseManager.SavePlayerScore(playerScore);
                Debug.Log("Puntaje guardado exitosamente.");
                NameInputField.interactable = false;
                return true; // Indicar que el puntaje se guard� correctamente
            }
            catch (ArgumentException e)
            {
                // Manejar el caso en que el nombre del jugador ya est� en la lista de puntajes
                Debug.LogError("Error al guardar el puntaje: " + e.Message);
                NameInputField.interactable = true;
                return false; // Indicar que hubo un error al guardar el puntaje
            }
        }
        else
        {
            Debug.LogWarning("Nombre de jugador inv�lido. Aseg�rate de ingresar un nombre no vac�o y �nico.");
            NameInputField.interactable = true;
            return false; // Indicar que hubo un error al guardar el puntaje
        }
    }

    // Genera un ID �nico para el jugador
    private int GenerateUniquePlayerId()
    {
        // Cargar los puntajes de los jugadores para determinar el �ltimo ID utilizado
        List<PlayerScoreData> playerScores = DatabaseManager.LoadPlayerScores();

        // Obtener el �ltimo ID utilizado sumando 1 al ID del �ltimo jugador
        int lastPlayerId = 0;
        if (playerScores.Count > 0)
        {
            lastPlayerId = playerScores[playerScores.Count - 1].Id;
        }
        int uniqueId = lastPlayerId + 1;

        return uniqueId;
    }

    private bool IsValidPlayerName(string playerName)
    {
        // Verificar si el nombre del jugador no est� en blanco o contiene solo espacios en blanco
        if (string.IsNullOrWhiteSpace(playerName))
        {
            return false;
        }

        // Convertir todos los nombres de los jugadores a min�sculas para comparar sin distinci�n entre may�sculas y min�sculas
        playerName = playerName.ToLower();

        // Cargar los puntajes de los jugadores y verificar si el nombre del jugador ya est� en la lista
        foreach (PlayerScoreData playerScore in DatabaseManager.LoadPlayerScores())
        {
            if (string.Equals(playerScore.PlayerName, playerName, StringComparison.OrdinalIgnoreCase))
            {
                //throw new ArgumentException("El nombre del jugador ya est� en la lista de puntajes.");]\
                return false;
            }
        }

        return true;
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = $"Score: {score}";
    }
}
