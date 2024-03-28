using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    private const string playerScoresFileName = "PlayerScores.json";
    private static string playerScoresFilePath;

    private void Awake()
    {
        // La ruta del archivo de puntajes del jugador
        playerScoresFilePath = Path.Combine(Application.streamingAssetsPath, playerScoresFileName);
    }

    // Guardar los puntajes de los jugadores
    public static void SavePlayerScores(List<PlayerScoreData> playerScores)
    {
        // Ordenar los puntajes de mejor a peor
        playerScores.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Convertir la lista de puntajes a JSON
        string jsonData = JsonUtility.ToJson(new PlayerScoreDataWrapper(playerScores));

        // Escribir el JSON en el archivo
        File.WriteAllText(playerScoresFilePath, jsonData);
    }

    // Cargar los puntajes de los jugadores
    public static List<PlayerScoreData> LoadPlayerScores()
    {
        // Verificar si existe el archivo de puntajes
        if (File.Exists(playerScoresFilePath))
        {
            // Leer el JSON del archivo
            string jsonData = File.ReadAllText(playerScoresFilePath);

            // Convertir el JSON a un objeto PlayerScoreDataWrapper
            PlayerScoreDataWrapper dataWrapper = JsonUtility.FromJson<PlayerScoreDataWrapper>(jsonData);

            // Retornar los puntajes ordenados de mejor a peor
            return dataWrapper.playerScores.OrderByDescending(scoreData => scoreData.Score).ToList();
        }
        else
        {
            // Si no existe el archivo de puntajes, retornar una lista vacía
            return new List<PlayerScoreData>();
        }
    }

    // Clase auxiliar para envolver la lista de puntajes
    [System.Serializable]
    private class PlayerScoreDataWrapper
    {
        public List<PlayerScoreData> playerScores;

        public PlayerScoreDataWrapper(List<PlayerScoreData> playerScores)
        {
            this.playerScores = playerScores;
        }
    }
}
