using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonRemapController : MonoBehaviour
{
    [SerializeField] private InputActionReference playerMoveAction;

    public void RemapInput(InputActionReference actionReference)
    {
        if (actionReference != null)
        {

        }
    }
} 