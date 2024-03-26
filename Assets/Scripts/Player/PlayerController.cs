using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private BezierCurve bezierCurve;
    private float t = 0.5f; // Inicia en el punto medio de la curva
    private GameControls controls;
    private float direction;

    private void Awake()
    {
        controls = new GameControls();
        bezierCurve = FindObjectOfType<BezierCurve>();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => { direction = ctx.ReadValue<float>(); };
        controls.Player.Move.canceled += ctx => { direction = 0f; };
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.canceled -= ctx => { direction = 0f; };
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        t += direction * movementSpeed * Time.deltaTime;

        // Asegúrese de que t esté en el rango [0, 1]
        t = Mathf.Clamp01(t);

        Vector3 newPosition = bezierCurve.GetPoint(bezierCurve.startPoint.position,
                                                    bezierCurve.controlPoint1.position,
                                                    bezierCurve.controlPoint2.position,
                                                    bezierCurve.endPoint.position,
                                                    t);

        transform.position = newPosition;
    }
}
