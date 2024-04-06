using UnityEngine;

public abstract class Brick : MonoBehaviour
{
    public delegate void OnBrickDestroyedDelegate(GameObject brick);
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
            hitsRemaining = 0;
        }
    }

    protected virtual void DestroyBrick()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        HandleSpecialBrick();

        AudioManager.Instance.PlaySoundOnObject(this.gameObject, SoundName.SFX_DestroyBrick);

        ScoreController scoreController = FindFirstObjectByType<ScoreController>();
        scoreController.AddScore(scoreAmount);

        OnBrickDestroyed?.Invoke(this.gameObject);
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
}
