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
    [SerializeField] private GameObject scoreboardPrefab;
    [SerializeField] private GameObject settingsPrefab;
    private GameObject menuInstance;
    private GameObject scoreboardInstance;
    private GameObject settingsInstance;

    private GameControls controls;

    private void Awake()
    {
        // Verificar si ya hay una instancia del menú en la escena
        if (menuPrefab != null && GameObject.Find(menuPrefab.name) == null)
        {
            CreateMenuInstance();
        }

        if (scoreboardPrefab != null && GameObject.Find(scoreboardPrefab.name) == null)
        {
            CreateScoreboardInstance();
        }

        if (settingsPrefab != null && GameObject.Find(settingsPrefab.name) == null)
        {
            CreateSettingsInstance();
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

    private void CreateScoreboardInstance()
    {
        // Verificar si hay un Canvas en la escena
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Si no hay Canvas, crea uno
            GameObject canvasGO = new GameObject("Scoreboard_Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Instanciar el scoreboard dentro del Canvas
        scoreboardInstance = Instantiate(scoreboardPrefab, canvas.transform);
        scoreboardInstance.SetActive(false); // El scoreboard no se mostrará al principio
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
        scoreboardInstance?.SetActive(false);
        settingsInstance?.SetActive(false);
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
        if (scoreboardInstance == null)
        {
            CreateScoreboardInstance();
        }

        // Mostrar el scoreboard y ocultar el menú y el menú de configuración
        scoreboardInstance?.SetActive(true);
        menuInstance?.SetActive(false);
        settingsInstance?.SetActive(false);

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
        if (settingsInstance == null)
        {
            CreateSettingsInstance();
        }

        // Mostrar el menú de configuración y ocultar el menú y el scoreboard
        settingsInstance?.SetActive(true);
        menuInstance?.SetActive(false);
        scoreboardInstance?.SetActive(false);

        GameManager.Instance.OptionsState();
    }
    private void CreateSettingsInstance()
    {
        // Verificar si hay un Canvas en la escena
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Si no hay Canvas, crea uno
            GameObject canvasGO = new GameObject("Settings_Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Instanciar el menú de configuración dentro del Canvas
        settingsInstance = Instantiate(settingsPrefab, canvas.transform);
        settingsInstance.SetActive(false); // El menú de configuración no se mostrará al principio
    }

    public void BackToMenu()
    {
        // Ocultar todos los menús
        menuInstance?.SetActive(false);
        scoreboardInstance?.SetActive(false);
        settingsInstance?.SetActive(false);
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
