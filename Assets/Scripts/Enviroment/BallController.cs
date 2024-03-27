using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f; // Velocidad inicial de la bola
    public float maxSpeed = 10f; // Velocidad máxima permitida
    public float minSpeed = 2f; // Velocidad mínima permitida
    public float speedIncreaseAmount = 2f; // Cantidad de aumento de velocidad al golpear al jugador
    public float speedIncreaseDuration = 2f; // Duración del aumento de velocidad en segundos
    public float speedDecreaseRate = 0.5f; // Tasa de disminución de velocidad por segundo
    public float downwardForce = 2f; // Fuerza hacia abajo para evitar que la bola quede atrapada

    private Rigidbody2D rb;
    private float currentSpeed;
    private bool speedIncreased = false;
    private float speedIncreaseTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
        // Asignar velocidad inicial a la bola
        rb.velocity = Vector2.up.normalized * currentSpeed;
    }

    void FixedUpdate()
    {
        // Disminuir gradualmente la velocidad con el tiempo hasta alcanzar el mínimo
        currentSpeed = Mathf.Max(minSpeed, currentSpeed - speedDecreaseRate * Time.fixedDeltaTime);

        // Aplicar fuerza hacia abajo para evitar que la bola quede atrapada en un bucle
        if (transform.position.y > 3.5f)
        {
            rb.AddForce(Vector2.down * downwardForce, ForceMode2D.Force);
        }

        // Controlar el aumento temporal de velocidad
        if (speedIncreased)
        {
            speedIncreaseTimer += Time.fixedDeltaTime;
            if (speedIncreaseTimer >= speedIncreaseDuration)
            {
                // Restaurar la velocidad original si se ha alcanzado el tiempo máximo de aumento
                currentSpeed = speed;
                speedIncreased = false;
                speedIncreaseTimer = 0f;
            }
        }

        // Aplicar la velocidad actualizada
        rb.velocity = rb.velocity.normalized * currentSpeed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Si la bola golpea al jugador, aumentar temporalmente la velocidad
        if (other.gameObject.CompareTag("Player"))
        {
            // Calcular la dirección del vector entre el punto de contacto y el centro de la bola
            Vector2 hitDirection = (other.contacts[0].point - (Vector2)transform.position).normalized;
            // Aplicar la nueva velocidad en la dirección del golpe del jugador
            rb.velocity = hitDirection * currentSpeed;
            // Aumentar temporalmente la velocidad
            currentSpeed += speedIncreaseAmount;
            // Limitar la velocidad máxima
            currentSpeed = Mathf.Min(maxSpeed, currentSpeed);
            speedIncreased = true;
        }
    }
}
