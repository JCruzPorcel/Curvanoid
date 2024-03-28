using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(MenuManager).Name;
                    instance = obj.AddComponent<MenuManager>();
                }
            }
            return instance;
        }
    }

    private bool isPaused = false;
    private int currentLevel = 0;

    [SerializeField] private GameObject menuPrefab;
    private GameObject menuInstance;
    private GameControls controls;

    private void Awake()
    {
        // Verificar si ya hay una instancia del menú en la escena
        if (menuPrefab != null && GameObject.Find(menuPrefab.name) == null)
        {
            CreateMenuInstance();
        }
    }

    private void Start()
    {
        // Obtener las instancias de los controles del GameManager
        controls = GameManager.Instance.Controls;

        // Habilitar los controles y asignar el evento de pausa
        controls.Enable();
        controls.Player.Pause.performed += ctx =>
        {
            if (CanToggleMenu())
            {
                PauseMenu();
            }
        };
    }

    private void CreateMenuInstance()
    {
        // Verificar si hay un Canvas en la escena
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Si no hay Canvas, crea uno
            GameObject canvasGO = new GameObject("MenuOptions_Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Instanciar el menú dentro del Canvas
        menuInstance = Instantiate(menuPrefab, canvas.transform);
        menuInstance.SetActive(isPaused);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            GameManager.Instance.OptionsState();
        }
        else
        {
            GameManager.Instance.InGameState();
        }
    }

    public void PauseMenu()
    {
        TogglePause();
        UpdateMenuInstance();
    }

    private void UpdateMenuInstance()
    {
        if (menuInstance == null)
        {
            CreateMenuInstance();
        }

        menuInstance?.SetActive(isPaused);
    }

    public void RestartLevel()
    {
        try
        {
            SceneManager.LoadScene($"Level_{currentLevel}");
        }
        catch (Exception ex)
        {
            SceneManager.LoadScene("MainMenu");
            Debug.Log($"Error, no se pudo cargar la escena: {ex}");
        }
    }

    public void Scoreboard()
    {
        GameManager.Instance.OptionsState();
    }

    public void MainMenu()
    {
        string sceneName = "MainMenu";

        DeactivateMenu();

        SwitchToScene(sceneName);

        GameManager.Instance.MainMenuState();
    }
    public void SetCurrentLevel(int newLevel)
    {
        currentLevel = newLevel;
    }

    private void DeactivateMenu()
    {
        if (menuInstance != null)
        {
            menuInstance.SetActive(false);
            isPaused = false;
        }
    }

    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private bool CanToggleMenu()
    {
        return GameManager.Instance.IsCurrentState(GameManager.GameState.InGame) || GameManager.Instance.IsCurrentState(GameManager.GameState.Options);
    }

    public void Settings()
    {

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
