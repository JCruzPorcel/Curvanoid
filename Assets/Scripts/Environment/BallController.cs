using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f; // Velocidad inicial de la bola
    [SerializeField] private float maxBurstSpeed = 20f; // Velocidad máxima durante el burst
    [SerializeField] private float burstDuration = 0.5f; // Duración del burst en segundos
    [SerializeField] private float decelerationRate = 5f; // Tasa de disminución de velocidad
    private Rigidbody2D rb;
    private bool isMoving = false;
    [SerializeField] private bool isBurstEnabled = true; // Controla si se activa el burst de velocidad
    private float burstEndTime = 0f;
    private Vector2 savedVelocity; // Guarda la velocidad antes de detener el movimiento

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Si estamos dentro del período de burst y el burst está habilitado
            if (Time.time < burstEndTime && isBurstEnabled)
            {
                // Aumentar temporalmente la velocidad hasta el máximo
                rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude + maxBurstSpeed * Time.fixedDeltaTime / burstDuration, maxBurstSpeed);
            }
            else
            {
                // Disminuir gradualmente la velocidad, asegurándose de que no baje del mínimo
                rb.velocity = rb.velocity.normalized * Mathf.Max(rb.velocity.magnitude - decelerationRate * Time.fixedDeltaTime, initialSpeed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si la bola golpea al jugador, activa el burst de velocidad
        if (collision.gameObject.CompareTag("Player"))
        {
            burstEndTime = Time.time + burstDuration;
        }
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.InGame)
        {
            ResumeMoving();
        }
        else
        {
            StopMoving();
        }
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            transform.parent = null;
            rb.velocity = transform.up * initialSpeed; // Establecer la velocidad inicial en la dirección actual

            isMoving = true;
        }
    }

    public void NewDirection(float angleInRadians)
    {
        transform.parent = null;
        Vector2 direction = Quaternion.Euler(0f, 0f, angleInRadians) * Vector2.up;
        rb.velocity = direction.normalized * initialSpeed; // Aplicar la nueva dirección
        isMoving = true;
    }

    private void StopMoving()
    {
        if (isMoving)
        {
            savedVelocity = rb.velocity; // Guardar la velocidad actual antes de detener el movimiento
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void ResumeMoving()
    {
        if (!isMoving && savedVelocity != Vector2.zero)
        {
            rb.velocity = savedVelocity; // Restaurar la velocidad guardada
            isMoving = true;
            savedVelocity = Vector2.zero; // Restablecer la velocidad guardada
        }
    }
}
