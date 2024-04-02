using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private InputActionReference skillActionReference;
    [SerializeField] private InputActionReference pauseActionReference;

    private void OnEnable()
    {
        DisableActions();
    }

    private void OnDisable()
    {
        EnableActions();
    }

    private void DisableActions()
    {
        moveActionReference.action.Disable();
        skillActionReference.action.Disable();
        pauseActionReference.action.Disable();
    }

    private void EnableActions()
    {
        moveActionReference.action.Enable();
        skillActionReference.action.Enable();
        pauseActionReference.action.Enable();
    }
}
