using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterBrick : Brick
{
    // ToDo: agregar el booster ya programado de la ball para que la impulse temporalmente

    // Temporalmente sera solo de incrementar Score

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreController scoreController = FindFirstObjectByType<ScoreController>();
        scoreController.AddScore(scoreAmount);
    }
}
