using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;

    #region Singleton
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
    #endregion

    #region Variables
    private bool isPaused = false;
    private int currentLevel = 0;

    [SerializeField] private GameObject menuPrefab;
    [SerializeField] private GameObject scoreboardPrefab;
    [SerializeField] private GameObject settingsPrefab;
    private GameObject menuInstance;
    private GameObject scoreboardInstance;
    private GameObject settingsInstance;

    private GameControls controls;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        CreateMenuInstanceIfNotFound(menuPrefab, menuPrefab.name, ref menuInstance);
        CreateMenuInstanceIfNotFound(scoreboardPrefab, scoreboardPrefab.name, ref scoreboardInstance);
        CreateMenuInstanceIfNotFound(settingsPrefab, settingsPrefab.name, ref settingsInstance);       
    }

    private void Start()
    {
        controls = GameManager.Instance.Controls;
        controls.Enable();
        controls.Player.Pause.performed += ctx =>
        {
            if (CanToggleMenu())
            {
                PauseMenu();
            }
        };
    }
    #endregion

    #region Menu Creation
    private void CreateMenuInstanceIfNotFound(GameObject prefab, string canvasName, ref GameObject instanceRef)
    {
        if (prefab != null && GameObject.Find(prefab.name) == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasGO = new GameObject(canvasName);
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            instanceRef = Instantiate(prefab, canvas.transform);
            instanceRef.SetActive(false);
        }
    }
    #endregion

    #region Menu Interactions
    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
            GameManager.Instance.OptionsState();
        else
            GameManager.Instance.InGameState();
    }

    public void PauseMenu()
    {
        TogglePause();
        UpdateMenuInstance();
    }

    private void UpdateMenuInstance()
    {
        if (menuInstance == null)
            CreateMenuInstanceIfNotFound(menuPrefab, menuPrefab.name, ref menuInstance);

        if (instance != null)
            menuInstance.SetActive(isPaused);

        settingsInstance?.SetActive(false);
        scoreboardInstance?.SetActive(false);
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
            CreateMenuInstanceIfNotFound(scoreboardPrefab, scoreboardPrefab.name, ref scoreboardInstance);

        if (instance != null)
            scoreboardInstance.SetActive(true);

        settingsInstance?.SetActive(false);
        menuInstance.SetActive(false);

        GameManager.Instance.OptionsState();
    }

    public void MainMenu()
    {
        string sceneName = "MainMenu";
        DesactivateMenu();
        SwitchToScene(sceneName);
        GameManager.Instance.MainMenuState();
    }

    public void SetCurrentLevel(int newLevel)
    {
        currentLevel = newLevel;
    }

    private void DesactivateMenu()
    {        
        menuInstance?.SetActive(false);
        isPaused = false;
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
            CreateMenuInstanceIfNotFound(settingsPrefab, settingsPrefab.name, ref settingsInstance);

        if (settingsInstance != null)
            settingsInstance.SetActive(true);

        scoreboardInstance?.SetActive(false);
        menuInstance.SetActive(false);

        GameManager.Instance.OptionsState();
    }

    public void BackToMenu()
    {
        DesactivateMenu();
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
    #endregion
}
