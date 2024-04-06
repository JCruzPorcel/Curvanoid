using UnityEngine;

public class RandomBrick : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    private int generatedBallCount = 3;
    [SerializeField] private SpriteRenderer iconContainer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            DestroyBrick();
        }
    }

    private void DestroyBrick()
    {
        HandleSpecialBrick();

        AudioManager.Instance.PlaySoundOnObject(this.gameObject, SoundName.SFX_DestroyBrick);

        iconContainer.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void HandleSpecialBrick()
    {
        generatedBallCount = Random.Range(2, 5);

        for (int i = 0; i < generatedBallCount; i++)
        {
            // Calcula un ángulo aleatorio entre -45 y 45 grados
            float angle = Random.Range(-45f, 45f);
            // Crea un cuaternión que representa una rotación sobre el eje Z
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            // Instancia la bola con la rotación inicial ajustada
            GameObject ballInstance = Instantiate(ballPrefab, transform.position, rotation);
            // Inicia el movimiento de la bola
            ballInstance.GetComponent<BallController>().StartMoving();
        }
    }
}
