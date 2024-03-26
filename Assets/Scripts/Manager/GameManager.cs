using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        InGame,
        Options,
        GameOver
    }

    public delegate void GameStateChangedHandler(GameState newState);
    public static event GameStateChangedHandler OnGameStateChanged;

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState currentGameState = GameState.MainMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetGameState(GameState newGameState)
    {
        if (currentGameState != newGameState)
        {
            currentGameState = newGameState;
            OnGameStateChanged?.Invoke(newGameState);
            Debug.Log($"Game state changed to: {currentGameState}");
        }
    }

    public void MainMenuState()
    {
        SetGameState(GameState.MainMenu);
    }

    public void InGameState()
    {
        SetGameState(GameState.InGame);
    }

    public void GameOverState()
    {
        SetGameState(GameState.GameOver);
    }

    public void OptionsState()
    {
        SetGameState(GameState.Options);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public bool IsCurrentState(GameState gameState)
    {
        return currentGameState == gameState;
    }
}
