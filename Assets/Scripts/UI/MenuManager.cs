using System;
using System.Collections;
using UnityEditor;
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

    public int currentLevel { get; private set; }
    public int maxLevels { get; private set; }

    [Header("Menu Prefabs")]
    [SerializeField] private GameObject pausePrefab;
    [SerializeField] private GameObject scoreboardPrefab;
    [SerializeField] private GameObject settingsPrefab;
    [SerializeField] private GameObject mainMenuPrefab;

    [Header("Win/Lose Prefabs")]
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private GameObject levelCompletPrefab;

    private GameObject pauseInstance;
    private GameObject scoreboardInstance;
    private GameObject settingsInstance;
    private GameObject mainMenuInstance;
    private GameObject gameOverInstance;
    private GameObject levelCompletInstance;

    private GameControls controls;
    #endregion

    #region MonoBehaviour Callbacks
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

        maxLevels = CountLevels();
    }
    #endregion

    #region Menu Creation
    [MenuItem("Tools/Count Levels")]
    private int CountLevels()
    {
        int levelCount = 0;

        // Iterar sobre todas las escenas en el proyecto
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            // Verificar si la escena no es el menú principal
            if (sceneName != "MainMenu")
            {
                levelCount++;
            }
        }

        //Debug.Log("Número de niveles (sin contar el menú principal): " + levelCount);

        return levelCount;
    }

    private void CreateMenuInstanceIfNotFound(GameObject prefab, ref GameObject instanceRef)
    {
        if (prefab != null && GameObject.Find(prefab.name) == null)
        {
            string canvasName = prefab.name;

            Canvas[] canvasArray = FindObjectsOfType<Canvas>();

            // Buscar el primer Canvas que no sea un descendiente del objeto actual
            Canvas canvas = Array.Find(canvasArray, c => c.gameObject != gameObject && !IsDescendantOf(c.gameObject, gameObject));

            // Si no se encontró un canvas que no sea un descendiente, crear uno nuevo
            if (canvas == null)
            {
                GameObject canvasGO = new GameObject(canvasName);
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            instanceRef = Instantiate(prefab, canvas.transform);
            instanceRef.name = canvasName;
            instanceRef.SetActive(false);
        }
    }

    // Función auxiliar para verificar si un objeto es descendiente de otro
    private bool IsDescendantOf(GameObject child, GameObject parent)
    {
        Transform t = child.transform.parent;
        while (t != null)
        {
            if (t.gameObject == parent) return true;
            t = t.parent;
        }
        return false;
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

    public void UpdateMenuInstance()
    {
        GetMainMenu();

        if (pauseInstance == null)
            CreateMenuInstanceIfNotFound(pausePrefab, ref pauseInstance);

        if (pauseInstance != null)
            pauseInstance.SetActive(isPaused);

        settingsInstance?.SetActive(false);
        scoreboardInstance?.SetActive(false);
    }

    public void RestartLevel()
    {
        try
        {
            SceneManager.LoadScene($"Level_{currentLevel}");
            GameManager.Instance.InGameState();
        }
        catch (Exception ex)
        {
            SceneManager.LoadScene("MainMenu");
            GameManager.Instance.MainMenuState();
            Debug.Log($"Error, no se pudo cargar la escena: {ex}");
        }

        isPaused = false;

        ClearMenuInstances();
    }

    public void Scoreboard()
    {
        GetMainMenu();

        if (scoreboardInstance == null)
            CreateMenuInstanceIfNotFound(scoreboardPrefab, ref scoreboardInstance);

        if (scoreboardInstance != null)
            scoreboardInstance.SetActive(true);

        settingsInstance?.SetActive(false);
        pauseInstance?.SetActive(false);
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

    public void NextLevel()
    {
        currentLevel++;

        if (currentLevel < maxLevels)
        {
            SwitchToScene($"Level_{currentLevel}");
        }
        else
        {
            MainMenu();
        }
    }

    private void DesactivateMenu()
    {
        pauseInstance?.SetActive(false);
        isPaused = false;
    }

    private void ClearMenuInstances()
    {
        settingsInstance = null;
        scoreboardInstance = null;
        pauseInstance = null;
    }

    public void SwitchToScene(string sceneName)
    {
        try
        {
            SceneManager.LoadScene(sceneName);
            GameManager.Instance.InGameState();
        }
        catch (Exception ex)
        {
            SceneManager.LoadScene("MainMenu"); // Puede reemplazarce por 0 para que cargue la primera escena para evitar errores (poner arriba del todo el main menu en dicho caso)
            GameManager.Instance.MainMenuState();
            Debug.Log($"Error, no se pudo cargar la escena: {ex}");
        }

        ClearMenuInstances();
    }

    private bool CanToggleMenu()
    {
        return GameManager.Instance.IsCurrentState(GameManager.GameState.InGame)
            || GameManager.Instance.IsCurrentState(GameManager.GameState.Options)
            && !TransitionManager.Instance.IsTransitioning();
    }

    public void Settings()
    {
        GetMainMenu();

        if (settingsInstance == null)
            CreateMenuInstanceIfNotFound(settingsPrefab, ref settingsInstance);

        if (settingsInstance != null)
            settingsInstance.SetActive(true);

        scoreboardInstance?.SetActive(false);
        pauseInstance?.SetActive(false);
    }

    public void GetMainMenu()
    {
        if (GameManager.Instance.IsCurrentState(GameManager.GameState.MainMenu))
        {
            if (mainMenuInstance == null)
                mainMenuInstance = GameObject.Find(mainMenuPrefab.name);

            if (mainMenuInstance != null)
            {
                mainMenuInstance.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"{mainMenuPrefab.name} not found!");
            }
        }
    }

    public void BackToMenu()
    {
        if (GameManager.Instance.IsCurrentState(GameManager.GameState.MainMenu))
        {
            mainMenuInstance?.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (scoreboardInstance == null)
            CreateMenuInstanceIfNotFound(gameOverPrefab, ref gameOverInstance);

        if (instance != null)
            gameOverInstance.SetActive(true);

        gameOverInstance.transform.parent = null;
        GameManager.Instance.GameOverState();
    }

    public void LevelComplete()
    {
        if (scoreboardInstance == null)
            CreateMenuInstanceIfNotFound(levelCompletPrefab, ref levelCompletInstance);


        if (instance != null)
            levelCompletInstance.SetActive(true);

        levelCompletInstance.transform.parent = null;
       GameManager.Instance.LevelCompletedState();
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
