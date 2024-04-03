using System;
using UnityEngine;

public class ReinforcedBrick : Brick
{
    [SerializeField] private SpriteRenderer srContainer;

    protected override void Start()
    {
        base.Start();
    }

    public override void TrackHits()
    {
        srContainer.enabled = true;
        base.TrackHits();
    }
}
