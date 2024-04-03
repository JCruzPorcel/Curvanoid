using UnityEngine;

public class ConditionalBrick : Brick
{
    [SerializeField] protected SpriteRenderer iconSprite;

    private bool hasKey = false;

    private void OnEnable()
    {
        KeyController.OnKeyAcquired += KeyAcquiredHandler;
    }

    private void OnDisable()
    {
        KeyController.OnKeyAcquired -= KeyAcquiredHandler;
    }

    private void KeyAcquiredHandler(bool hasKey)
    {
        this.hasKey = hasKey;
        UpdateBrickState();

        if (!hasKey)
        {
            MenuManager.Instance.GameOver();
        }
    }

    protected override void DestroyBrick()
    {
        if (hasKey)
        {
            base.DestroyBrick();
        }
    }

    private void UpdateBrickState()
    {
        if (iconSprite != null)
        {
            iconSprite.enabled = !hasKey;
        }
    }
}
