using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private BezierCurve bezierCurve;
    private float t = 0.5f; // Inicia en el punto medio de la curva
    private GameControls controls;
    private bool canMove = false;

    public GameControls Controls { get { return controls; } private set => controls = value; }
    private float direction;

    private BallController ballController;

    private void Awake()
    {
        controls = new GameControls();
        bezierCurve = FindObjectOfType<BezierCurve>();
        ballController = GetComponentInChildren<BallController>();
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
        controls.Player.Skill.performed -= ctx => StartGame();
    }

    private void FixedUpdate()
    {
        if (canMove)
            HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        t += direction * movementSpeed * Time.fixedDeltaTime;

        // Aseg�rese de que t est� en el rango [0, 1]
        t = Mathf.Clamp01(t);

        Vector3 newPosition = bezierCurve.GetPoint(t);
        transform.position = newPosition;

        // Si t est� cerca del final de la curva, no calculamos la direcci�n y rotaci�n
        if (t < 1f)
        {
            // Calcular la direcci�n y rotaci�n basada en la tangente en el punto actual
            Vector3 tangent = bezierCurve.GetTangent(t);
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = targetRotation;
        }
    }

    private void StartGame()
    {
        /*float playerRotation = transform.eulerAngles.y;
        float angleInRadians = Mathf.Deg2Rad * (90f - playerRotation); // Ajuste del �ngulo para que sea en el plano XY*/
        ballController?.StartMoving();
        canMove = true;
    }
}
