using UnityEngine;

public abstract class Brick : MonoBehaviour
{
    public delegate void OnBrickDestroyedDelegate();
    public static event OnBrickDestroyedDelegate OnBrickDestroyed;

    [SerializeField] protected int scoreAmount = 10;
    [SerializeField] protected int hitsRemaining = 1;
    [SerializeField] protected bool useRandomColorOnStart;

    protected virtual void HandleSpecialBrick() { }

    public virtual void TrackHits()
    {
        hitsRemaining--;
        if (hitsRemaining <= 0)
        {
            DestroyBrick();
        }
    }

    protected virtual void DestroyBrick()
    {
        HandleSpecialBrick();

        OnBrickDestroyed?.Invoke();

        ScoreController scoreController = FindFirstObjectByType<ScoreController>();
        scoreController.AddScore(scoreAmount);

        gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        if (useRandomColorOnStart)
        {
            SetRandomColor();
        }
    }

    private void SetRandomColor()
    {
        Color newColor = new Color(Random.value, Random.value, Random.value);
        GetComponent<SpriteRenderer>().color = newColor;
    }

    protected void OnBrickDestroyedInvoke()
    {
        OnBrickDestroyed?.Invoke();
    }
}
