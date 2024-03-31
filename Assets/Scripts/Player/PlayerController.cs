using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private float direction;
    private float t = 0.5f; // Inicia en el punto medio de la curva
    private bool canMove = false;

    private BezierCurve bezierCurve;
    private GameControls controls;

    private void Awake()
    {
        bezierCurve = FindFirstObjectByType<BezierCurve>();
        controls = GameManager.Instance.Controls;
    }

    private void Start()
    {
        transform.position = bezierCurve.GetPoint(t);
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += ctx => { direction = ctx.ReadValue<float>(); };
        controls.Player.Move.canceled += ctx => { direction = 0f; };

        controls.Player.Skill.performed += ctx => StartGame();
    }

    private void OnDisable()
    {
        controls.Disable();

        controls.Player.Move.canceled -= ctx => { direction = 0f; };

        controls.Player.Skill.canceled -= ctx => StartGame();
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

    private void StartGame()
    {
        if (canMove && !GameManager.Instance.IsCurrentState(GameManager.GameState.InGame)) return;

        BallController ballController = GetComponentInChildren<BallController>();

        ballController?.StartMoving();

        canMove = true;
    }
}
