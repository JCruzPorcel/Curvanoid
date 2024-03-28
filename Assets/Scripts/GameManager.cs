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
    [SerializeField] private GameState currentGameState = GameState.MainMenu;

    public delegate void GameStateChangedHandler(GameState newState);
    public static event GameStateChangedHandler OnGameStateChanged;

    [SerializeField] private GameObject persistentObject;
    public static GameManager Instance { get; private set; }
    public GameControls Controls { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(persistentObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        currentGameState = GameState.MainMenu;
        Controls = new GameControls();
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
        Cursor.lockState = CursorLockMode.None;
    }

    public void InGameState()
    {
        SetGameState(GameState.InGame);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GameOverState()
    {
        SetGameState(GameState.GameOver);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OptionsState()
    {
        SetGameState(GameState.Options);
        Cursor.lockState = CursorLockMode.None;
    }

    public bool IsCurrentState(GameState gameState)
    {
        return currentGameState == gameState;
    }

    // Si se requiere implementar logica adicional, reemplazar los Gettters y Setters de Controls por esta funcion.
    /*public GameControls GetGameControls()
    {
        return controls;
    }*/
}

