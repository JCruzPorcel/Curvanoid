using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f; // Velocidad inicial de la bola
    [SerializeField] private float maxBurstSpeed = 20f; // Velocidad m�xima durante el burst
    [SerializeField] private float burstDuration = 0.5f; // Duraci�n del burst en segundos
    [SerializeField] private float decelerationRate = 5f; // Tasa de disminuci�n de velocidad
    private Rigidbody2D rb;
    private bool isMoving = false;
    private float burstEndTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            transform.parent = null;
            rb.velocity = transform.up * initialSpeed; // Establecer la velocidad inicial en la direcci�n actual
            isMoving = true;
        }
    }

    public void NewDirection(float angleInRadians)
    {
        transform.parent = null;
        Vector2 direction = Quaternion.Euler(0f, 0f, angleInRadians) * Vector2.up;
        rb.velocity = direction.normalized * initialSpeed; // Aplicar la nueva direcci�n
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Si estamos dentro del per�odo de burst
            if (Time.time < burstEndTime)
            {
                // Aumentar temporalmente la velocidad hasta el m�ximo
                rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude + maxBurstSpeed * Time.fixedDeltaTime / burstDuration, maxBurstSpeed);
            }
            else
            {
                // Disminuir gradualmente la velocidad, asegur�ndote de que no baje del m�nimo
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
        }else if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
