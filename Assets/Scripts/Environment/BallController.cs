using UnityEngine;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shockwave_Prefab; // Prefab del shockwave
    [SerializeField] private float initialSpeed = 10f; // Velocidad inicial de la bola
    [SerializeField] private float maxBurstSpeed = 20f; // Velocidad máxima durante el burst
    [SerializeField] private float burstDuration = 0.5f; // Duración del burst en segundos
    [SerializeField] private float decelerationRate = 5f; // Tasa de disminución de velocidad
    [SerializeField] private bool isBurstEnabled = true; // Controla si se activa el burst de velocidad
    private float burstEndTime = 0f;
    private bool isMoving = false;
    private Vector2 savedVelocity; // Guarda la velocidad antes de detener el movimiento

    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private List<ParticleSystem> shockwavePool = new List<ParticleSystem>(); // Pool de shockwaves

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
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
        // Calcula la fuerza del impacto usando la velocidad relativa de la colisión
        float impactForce = collision.relativeVelocity.magnitude;

        // Define un umbral para considerar que el impacto es lo suficientemente fuerte
        float impactThreshold = 4.5f;

        // Comprueba si la fuerza del impacto supera el umbral
        if (impactForce > impactThreshold)
        {
            // Reproduce el sonido de impacto solo si el impacto es lo suficientemente fuerte
            AudioManager.Instance.PlaySoundOnObject(this.gameObject, SoundName.SFX_BallImpact); // Se obtiene desde el Utils.JCruzPorcel.AudioManager

            if (collision.gameObject.CompareTag("Brick") || collision.gameObject.CompareTag("SpecialBrick"))
            {
                collision.gameObject.GetComponent<Brick>().TrackHits();
            }
        }

        // Si la bola golpea al jugador, activa el burst de velocidad y el shockwave
        if (collision.gameObject.CompareTag("Player"))
        {
            burstEndTime = Time.time + burstDuration;

            // Intenta obtener un shockwave inactivo del pool
            ParticleSystem shockwave = GetInactiveShockwave();

            if (shockwave == null)
            {
                // Si no hay shockwave inactivo, instanciar uno nuevo
                shockwave = Instantiate(shockwave_Prefab, transform.position, Quaternion.identity);
                shockwavePool.Add(shockwave);
            }

            shockwave.transform.position = transform.position;

            shockwave.Play();
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
        if (!isMoving && GameManager.Instance.IsCurrentState(GameManager.GameState.InGame))
        {
            circleCollider.enabled = true;
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
            circleCollider.enabled = false;
            savedVelocity = rb.velocity; // Guardar la velocidad actual antes de detener el movimiento
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void ResumeMoving()
    {
        if (!isMoving && savedVelocity != Vector2.zero)
        {
            circleCollider.enabled = true;
            rb.velocity = savedVelocity; // Restaurar la velocidad guardada
            isMoving = true;
            savedVelocity = Vector2.zero; // Restablecer la velocidad guardada
        }
    }

    private ParticleSystem GetInactiveShockwave()
    {
        foreach (ParticleSystem shockwave in shockwavePool)
        {
            if (!shockwave.isPlaying)
            {
                return shockwave;
            }
        }
        return null;
    }
}
