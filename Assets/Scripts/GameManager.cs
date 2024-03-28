using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    #region Enums
    public enum GameState
    {
        MainMenu,
        InGame,
        Options,
        GameOver
    }
    #endregion

    #region Variables
    [SerializeField] private GameState currentGameState = GameState.MainMenu;
    [SerializeField] private GameObject persistentObject;
    public static GameManager Instance { get; private set; }
    public GameControls Controls { get; private set; }
    #endregion

    #region Events
    public delegate void GameStateChangedHandler(GameState newState);
    public static event GameStateChangedHandler OnGameStateChanged;
    #endregion

    #region MonoBehaviour Callbacks
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
    #endregion

    #region Game State Methods
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
    #endregion

    // Si se requiere implementar lógica adicional, reemplazar los Getters y Setters de Controls por esta función.
    /*public GameControls GetGameControls()
    {
        return controls;
    }*/
}
