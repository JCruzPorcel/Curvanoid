using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private const string playerScoresFileName = "PlayerScores.json";
    private static string playerScoresFilePath;

    private void Awake()
    {
        // La ruta del directorio donde se guarda el archivo de puntajes del jugador
        string directoryPath = Path.Combine(Application.streamingAssetsPath, "Database");

        // Verificar si el directorio no existe
        if (!Directory.Exists(directoryPath))
        {
            // Si el directorio no existe, crearlo
            Directory.CreateDirectory(directoryPath);
        }

        // Establecer la ruta del archivo de puntajes del jugador
        playerScoresFilePath = Path.Combine(directoryPath, playerScoresFileName);
    }

    // Guardar el puntaje de un solo jugador
    public static void SavePlayerScore(PlayerScoreData playerScore)
    {
        // Obtener los puntajes de los jugadores guardados previamente
        List<PlayerScoreData> savedPlayerScores = LoadPlayerScores();

        // Agregar el nuevo puntaje a la lista
        savedPlayerScores.Add(playerScore);

        // Ordenar los puntajes de mejor a peor
        savedPlayerScores.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Convertir la lista de puntajes a JSON
        string jsonData = JsonUtility.ToJson(new PlayerScoreDataWrapper(savedPlayerScores));

        // Escribir el JSON en el archivo
        File.WriteAllText(playerScoresFilePath, jsonData);
    }

    // Cargar los puntajes de los jugadores
    public static List<PlayerScoreData> LoadPlayerScores()
    {
        // Verificar si existe el archivo de puntajes
        if (!File.Exists(playerScoresFilePath))
        {
            // Si el archivo no existe, crear uno nuevo con una lista vacía de puntajes
            CreateEmptyScoreFile();
        }

        // Leer el JSON del archivo
        string jsonData = File.ReadAllText(playerScoresFilePath);

        // Convertir el JSON a un objeto PlayerScoreDataWrapper
        PlayerScoreDataWrapper dataWrapper = JsonUtility.FromJson<PlayerScoreDataWrapper>(jsonData);

        // Verificar si se pudo deserializar correctamente el JSON
        if (dataWrapper == null || dataWrapper.playerScores == null)
        {
            Debug.LogWarning("No se pudieron cargar los puntajes de los jugadores.");
            // Retornar una lista vacía
            return new List<PlayerScoreData>();
        }

        // Retornar los puntajes ordenados de mejor a peor
        return dataWrapper.playerScores.OrderByDescending(scoreData => scoreData.Score).ToList();
    }

    // Crear un archivo de puntajes vacío si no existe
    private static void CreateEmptyScoreFile()
    {
        List<PlayerScoreData> emptyScores = new List<PlayerScoreData>();
        string jsonData = JsonUtility.ToJson(new PlayerScoreDataWrapper(emptyScores));
        File.WriteAllText(playerScoresFilePath, jsonData);
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
