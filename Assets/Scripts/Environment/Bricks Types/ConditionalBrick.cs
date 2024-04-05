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

    private void KeyAcquiredHandler(bool m_hasKey)
    {
        this.hasKey = m_hasKey;
        iconSprite.enabled = !hasKey;

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
}
