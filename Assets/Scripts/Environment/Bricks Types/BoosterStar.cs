using UnityEngine;

public class BoosterStar : MonoBehaviour
{
    [SerializeField] private int scoreAmount;

    // ToDo: agregar el booster ya programado de la ball para que la impulse temporalmente

    // Temporalmente sera solo de incrementar Score

    private void PickStar()
    {
        ScoreController scoreController = FindFirstObjectByType<ScoreController>();
        scoreController.AddScore(scoreAmount);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            PickStar();
        }
    }
}
