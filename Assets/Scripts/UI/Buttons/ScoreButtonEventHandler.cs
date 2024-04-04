using TMPro;
using UnityEngine;

public class ScoreButtonEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject saveScoreButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject levelButton;
    [SerializeField] private GameObject alertMessage;
    [SerializeField] private GameObject scoreboardPanel;
    [SerializeField] private TMP_InputField name_InputField;
    [SerializeField] private TextMeshProUGUI score_Text;
    [SerializeField] private TextMeshProUGUI header_Text;
    [SerializeField, TextArea(5, 15)] private string headerStringText;

    private ScoreController scoreController;

    private void Start()
    {
        scoreController = FindFirstObjectByType<ScoreController>(); // Encontrar el ScoreController en la escena

        if (scoreController != null)
        {
            scoreController.NameInputField = name_InputField; // Asignar el InputField al ScoreController
            score_Text.text = $"Score: {scoreController.score}";
        }
        else
        {
            Debug.LogError("ScoreController no encontrado en la escena.");
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (GameManager.Instance.IsCurrentState(GameManager.GameState.LevelCompleted))
        {
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Siguiente Nivel";
        }
        else if (GameManager.Instance.IsCurrentState(GameManager.GameState.GameOver))
        {
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Reiniciar";
        }
    }

    public void OnSaveScoreButtonClick()
    {
        if (scoreController == null)
        {
            Debug.LogError("ScoreController no encontrado en la escena.");
            return;
        }

        if (scoreController.SaveScore())
        {
            // Si el puntaje se guardó correctamente, desactivar el botón de guardar puntaje
            header_Text.text = headerStringText;

            if (MenuManager.Instance.currentLevel + 1 >= MenuManager.Instance.maxLevels)
            {
                header_Text.text = "¡Felicidades! \n ¡Haz completado todos los \n niveles disponibles!";

                saveScoreButton.SetActive(false);
                mainMenuButton.SetActive(false);
                levelButton.SetActive(false);

                // Crear una instancia del botón de menú principal
                mainMenuButton.SetActive(true);

                // Copiar las propiedades de transformación del botón saveScoreButton al nuevo botón
                // RectTransform saveScoreRectTransform = saveScoreButton.GetComponent<RectTransform>();
                RectTransform mainMenuButtonRectTransform = mainMenuButton.GetComponent<RectTransform>();

                /*newButtonRectTransform.anchorMin = saveScoreRectTransform.anchorMin;
                  newButtonRectTransform.anchorMax = saveScoreRectTransform.anchorMax;
                  newButtonRectTransform.pivot = saveScoreRectTransform.pivot;
                  newButtonRectTransform.anchoredPosition = saveScoreRectTransform.anchoredPosition;*/

                mainMenuButtonRectTransform.anchorMin = new Vector2(0.5f, 0f);
                mainMenuButtonRectTransform.anchorMax = new Vector2(0.5f, 0f);
                mainMenuButtonRectTransform.pivot = new Vector2(0.5f, 0f);
                mainMenuButtonRectTransform.anchoredPosition = new Vector2(0f, 15f);
            }
            else
            {
                saveScoreButton.SetActive(false);
                mainMenuButton.SetActive(true);
                levelButton.SetActive(true);
            }

            scoreboardPanel.SetActive(true);
        }
        else
        {
            // Si hubo un error al guardar el puntaje, no hacer ningún cambio en los botones
            Debug.LogWarning("Hubo un error al guardar el puntaje.");
            alertMessage.SetActive(true);
        }
    }
}
