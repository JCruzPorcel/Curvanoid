using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject winPrefab;
    [SerializeField] private GameObject losePrefab;
    [Space(10)]
    [SerializeField, Tooltip("Transform del GameObject ART para guardar la instancia del Jugador")]
    private Transform artTransform;
    private Transform ballPosition;

    // Lista para almacenar todos los bricks
    private List<GameObject> bricks = new List<GameObject>();

    public int InitialBallCount { get; set; } = 1;
    public int RemainingBricks { get; set; }

    private void Start()
    {
        Brick.OnBrickDestroyed += RemoveBrickFromList;

        // Buscar todos los bricks y agregarlos a la lista
        GameObject[] brickObjects = GameObject.FindGameObjectsWithTag("Brick");
        foreach (GameObject brick in brickObjects)
        {
            bricks.Add(brick);
        }

        RemainingBricks = bricks.Count;

        GameObject playerPos = Instantiate(playerPrefab, artTransform);
        ballPosition = Instantiate(ballPrefab, playerPos.transform).transform; // Creamos y obtenemos el transform de Ball
    }

    private void OnDisable()
    {
        Brick.OnBrickDestroyed -= RemoveBrickFromList;
    }

    private void Update()
    {
        if (GameManager.Instance.IsCurrentState(GameManager.GameState.InGame))
        {
            CheckLoseCondition();
        }
    }

    private void HandleBrickDestroyed()
    {
        CheckWinCondition();
        Debug.Log("Remaining bricks: " + RemainingBricks);
    }

    private void RemoveBrickFromList(GameObject brick)
    {
        bricks.Remove(brick);
        RemainingBricks = bricks.Count;
        HandleBrickDestroyed();
    }

    private void CheckWinCondition()
    {
        if (RemainingBricks == 0)
        {
            // Debug.Log("¡Has ganado!");
            MenuManager.Instance.LevelComplete();
        }
    }

    private void CheckLoseCondition()
    {
        if (BallOutOfBounds())
        {
            InitialBallCount--;
        }

        if (InitialBallCount <= 0)
        {
            // Debug.Log("¡Has perdido!");
            MenuManager.Instance.GameOver();
        }
    }

    private bool BallOutOfBounds()
    {
        return ballPosition.position.y < ResolutionManager.BottomBoundary;
    }
}
