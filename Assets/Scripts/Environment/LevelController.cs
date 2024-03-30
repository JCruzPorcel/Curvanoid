using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerPrefab;
    [Space(10)]
    [SerializeField,Tooltip("Transform del GameObject ART para guardar la instancia del Jugador")] private Transform artTransform; // Para que este mas ordenada la jerarquia al momento de testear

    private Transform ballPosition;

    private int remainingBricks;

    private void Start()
    {
        Brick.OnBrickDestroyed += HandleBrickDestroyed;
        remainingBricks = FindObjectsOfType<Brick>().Length;

        GameObject playerPos =  Instantiate(playerPrefab, artTransform);

        ballPosition = Instantiate(ballPrefab, playerPos.transform).transform; // Creamos y obternemos el trasnform de Ball
    }

    private void OnDisable()
    {
        Brick.OnBrickDestroyed -= HandleBrickDestroyed;
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
        remainingBricks--;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (remainingBricks == 0)
        {
            Debug.Log("¡Has ganado!");
            GameManager.Instance.LevelCompletedState();
        }
    }

    private void CheckLoseCondition()
    {
        if (BallOutOfBounds())
        {
            Debug.Log("¡Has perdido!");
            GameManager.Instance.GameOverState();
        }
    }

    private bool BallOutOfBounds()
    {
        return ballPosition.position.y < ResolutionManager.BottomBoundary;
    }
}
