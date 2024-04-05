using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    #region Enums
    public enum GameState
    {
        MainMenu,
        Options,
        LevelCompleted,
        InGame,
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
        Controls = new GameControls();
        MainMenuState();
    }
    #endregion

    #region Game State Methods
    private void SetGameState(GameState newGameState)
    {
        if (currentGameState != newGameState)
        {
            currentGameState = newGameState;
            OnGameStateChanged?.Invoke(newGameState);
            //Debug.Log($"Game state changed to: {currentGameState}");

            switch (newGameState)
            {
                case GameState.MainMenu:
                    MainMenuState();
                    break;
                case GameState.Options:
                    OptionsState();
                    break;
                case GameState.LevelCompleted:
                    LevelCompletedState();
                    break;
                case GameState.InGame:
                    InGameState();
                    break;
                case GameState.GameOver:
                    GameOverState();
                    break;
            }
        }
    }

    public void MainMenuState()
    {
        if (TransitionManager.Instance.IsTransitioning())
        {
            StartCoroutine(WaitForTransitionAndLoadNextLevel(GameState.MainMenu));
            return;
        }

        SetGameState(GameState.MainMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void LevelCompletedState()
    {
        if (TransitionManager.Instance.IsTransitioning())
        {
            StartCoroutine(WaitForTransitionAndLoadNextLevel(GameState.LevelCompleted));
            return;
        }

        SetGameState(GameState.LevelCompleted);
        Cursor.lockState = CursorLockMode.None;
    }

    public void InGameState()
    {
        if (TransitionManager.Instance.IsTransitioning())
        {
            StartCoroutine(WaitForTransitionAndLoadNextLevel(GameState.InGame));
            return;
        }

        SetGameState(GameState.InGame);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GameOverState()
    {
        if (TransitionManager.Instance.IsTransitioning())
        {
            StartCoroutine(WaitForTransitionAndLoadNextLevel(GameState.GameOver));
            return;
        }

        SetGameState(GameState.GameOver);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OptionsState()
    {
        if (TransitionManager.Instance.IsTransitioning())
        {
            StartCoroutine(WaitForTransitionAndLoadNextLevel(GameState.Options));
            return;
        }

        SetGameState(GameState.Options);
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator WaitForTransitionAndLoadNextLevel(GameState newGameState)
    {
        while (TransitionManager.Instance.IsTransitioning())
        {
            yield return null;
        }

        SetGameState(newGameState);
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
