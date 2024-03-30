using UnityEngine;

public abstract class Brick : MonoBehaviour
{
    public delegate void OnBrickDestroyedDelegate();
    public static event OnBrickDestroyedDelegate OnBrickDestroyed;

    [SerializeField] protected int scoreAmount;
    protected int hitsRemaining = 1; // N�mero de golpes restantes antes de destruir el ladrillo

    protected virtual void HandleSpecialBrick() { }

    protected virtual void TrackHits()
    {
        hitsRemaining--; // Restar uno al n�mero de golpes restantes
        if (hitsRemaining <= 0)
        {
            DestroyBrick(); // Destruir el ladrillo si no quedan golpes restantes
        }
    }

    protected void DestroyBrick()
    {
        HandleSpecialBrick(); // Llamar al m�todo para manejar ladrillos especiales

        OnBrickDestroyed?.Invoke();

        // Todo: Agregar puntaje

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TrackHits(); // Llamar al m�todo para rastrear el n�mero de golpes
        }
    }
}
