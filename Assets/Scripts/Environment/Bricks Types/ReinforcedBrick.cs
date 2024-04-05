using UnityEngine;

public class ReinforcedBrick : Brick
{
    [SerializeField] private SpriteRenderer srContainer;

    protected override void Start()
    {
        base.Start();
    }

    protected override void DestroyBrick()
    {
        srContainer.enabled = false;
        base.DestroyBrick();
    }

    public override void TrackHits()
    {
        srContainer.enabled = true;
        hitsRemaining--;
        if (hitsRemaining <= 0)
        {
            DestroyBrick();
        }
    }
}
