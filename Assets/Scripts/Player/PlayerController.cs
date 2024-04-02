using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private float direction;
    private float t = 0.5f; // Inicia en el punto medio de la curva
    private bool canMove = false;

    private BezierCurve bezierCurve;
    // private GameControls controls;

    private void Start()
    {
        bezierCurve = FindFirstObjectByType<BezierCurve>();
        transform.position = bezierCurve.GetPoint(t);
    }

    private void OnEnable()
    {
        //controls = GameManager.Instance.Controls;

        //controls.Enable();

        //controls.Player.Move.performed += ctx => { direction = ctx.ReadValue<float>(); };
        //controls.Player.Move.canceled += ctx => { direction = 0f; };

        //controls.Player.Skill.performed += ctx => StartGame();
    }

    private void OnDestroy()
    {
        //controls.Disable();

        //controls.Player.Move.canceled -= ctx => { direction = 0f; };

        //controls.Player.Skill.canceled -= ctx => StartGame();
    }

    private void FixedUpdate()
    {
        if (canMove)
            HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (!GameManager.Instance.IsCurrentState(GameManager.GameState.InGame)) return;

        t += direction * movementSpeed * Time.fixedDeltaTime;

        // Asegúrese de que t esté en el rango [0, 1]
        t = Mathf.Clamp01(t);

        Vector3 newPosition = bezierCurve.GetPoint(t);
        transform.position = newPosition;

        // Si t está cerca del final de la curva, no calculamos la dirección y rotación
        if (t < 1f)
        {
            // Calcular la dirección y rotación basada en la tangente en el punto actual
            Vector3 tangent = bezierCurve.GetTangent(t);
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = targetRotation;
        }
    }

    public void StartGame(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!canMove && GameManager.Instance.IsCurrentState(GameManager.GameState.InGame))
            {
                BallController ballController = FindFirstObjectByType<BallController>();
                ballController?.StartMoving();

                canMove = true;
            }
        }
    }

    // Cambio de Behavior: De C# Events a Unity Events para poder rebindear las teclas
    // ToDo: Refactorizar codigos relacionados a los controles

    public void GetDirection(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<float>();
    }
}
