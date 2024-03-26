using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public bool IsMoving { get; set; } = true;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartMoving();
    }

    public void StartMoving()
    {
        // Generar una dirección aleatoria para el movimiento inicial
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;

        // Aplicar velocidad inicial a la bola
        rb.velocity = randomDirection * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si la bola colisiona con otro objeto, rebota
        if (!IsMoving) return;

        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflection = Vector3.Reflect(rb.velocity, normal);
        rb.velocity = reflection.normalized * speed;
    }
}
