using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonEventHandler : MonoBehaviour
{
    private GameObject menuToActivate;
    private GameObject menuToDeactivate;

    public void OnPauseButtonClick()
    {
        MenuManager.Instance.PauseMenu();
    }

    public void OnRestartLevelButtonClick()
    {
        StartTransitionAndExecute(MenuManager.Instance.RestartLevel);
    }

    public void OnScoreboardButtonClick()
    {
        if (IsInMainMenu())
        {
            StartTransitionAndExecute(MenuManager.Instance.Scoreboard);
        }
        else
        {
            MenuManager.Instance.Scoreboard();
        }
    }

    public void OnMainMenuButtonClick()
    {
        StartTransitionAndExecute(MenuManager.Instance.MainMenu);
        ScoreController.LastPlayerID = null;
    }

    public void OnSettingsButtonClick()
    {
        if (IsInMainMenu())
        {
            StartTransitionAndExecute(MenuManager.Instance.Settings);
        }
        else
        {
            MenuManager.Instance.Settings();
        }
    }

    public void OnBackToMenuButtonClick()
    {
        if (IsInMainMenu())
        {
            StartTransitionAndExecute(MenuManager.Instance.BackToMenu);
        }
        else
        {
            if (menuToDeactivate != null)
            {
                menuToDeactivate.SetActive(false);
                menuToDeactivate = null;
            }

            MenuManager.Instance.BackToMenu();
            MenuManager.Instance.UpdateMenuInstance();
        }
    }

    public void OnQuitGameButtonClick()
    {
        MenuManager.Instance.QuitGame();
    }

    public void OnSelectLevelButtonClick(int currentLevel)
    {
        StartTransitionAndExecute(() =>
        {
            MenuManager.Instance.SetCurrentLevel(currentLevel);
            MenuManager.Instance.SwitchToScene($"Level_{currentLevel}");
        });
    }

    public void OnPlayButtonClick()
    {
        StartTransitionAndExecute(MenuManager.Instance.GetMainMenu);
    }

    public void OnNextLevelButtonClick()
    {
        MenuManager.Instance.NextLevel();
    }

    public void SetMenuToActivate(GameObject newMenu)
    {
        menuToActivate = newMenu;
    }

    public void DeactivateCurrentMenu(GameObject currentMenu)
    {
        menuToDeactivate = currentMenu;
    }

    private bool IsInMainMenu()
    {
        return GameManager.Instance.IsCurrentState(GameManager.GameState.MainMenu);
    }

    private void StartTransitionAndExecute(Action action)
    {
        TransitionManager.Instance.StartTransition();
        StartCoroutine(ExecuteWithDelay(action));
    }

    private IEnumerator ExecuteWithDelay(Action action)
    {
        float delayBeforeExecution = TransitionManager.Instance.transitionTime / 2;

        yield return new WaitForSeconds(delayBeforeExecution);

        if (menuToActivate != null)
        {
            menuToActivate.SetActive(true);
            menuToActivate = null;
        }

        if (menuToDeactivate != null)
        {
            menuToDeactivate.SetActive(false);
            menuToDeactivate = null;
        }

        action.Invoke();
    }    
}

