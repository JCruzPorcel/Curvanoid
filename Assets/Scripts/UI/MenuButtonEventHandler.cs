using UnityEngine;

public class MenuButtonEventHandler : MonoBehaviour
{
    public void OnPauseButtonClick()
    {
        MenuManager.Instance.PauseMenu();
    }

    public void OnRestartLevelButtonClick()
    {
        MenuManager.Instance.RestartLevel();
    }

    public void OnScoreboardButtonClick()
    {
        MenuManager.Instance.Scoreboard();
    }

    public void OnMainMenuButtonClick()
    {
        MenuManager.Instance.MainMenu();
    }

    public void OnSettingsButtonClick()
    {
        MenuManager.Instance.Settings();
    }

    public void OnBackToMenuButtonClick()
    {
        MenuManager.Instance.BackToMenu();
    }

    public void OnQuitGameButtonClick()
    {
        MenuManager.Instance.QuitGame();
    }
}
