using UnityEngine;

public class RandomBrick : Brick
{
    [SerializeField] private GameObject ballPrefab;
    private int generatedBallCount = 3;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TrackHits();
        }
    }

    protected override void HandleSpecialBrick()
    {
        generatedBallCount = Random.Range(1, 4);

        for (int i = 0; i < generatedBallCount; i++)
        {
            GameObject ballInstance = Instantiate(ballPrefab, transform.position, Quaternion.identity);

            ballInstance.GetComponent<BallController>().StartMoving();
        }
    }
}
