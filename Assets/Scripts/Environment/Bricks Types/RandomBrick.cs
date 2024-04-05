using UnityEngine;

public class RandomBrick : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    private int generatedBallCount = 3;
    [SerializeField] private SpriteRenderer iconContainer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            DestroyBrick();
        }
    }

    private void DestroyBrick()
    {
        HandleSpecialBrick();

        AudioManager.Instance.PlaySoundOnObject(this.gameObject, SoundName.SFX_DestroyBrick);

        iconContainer.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void HandleSpecialBrick()
    {
        generatedBallCount = Random.Range(2, 5);

        for (int i = 0; i < generatedBallCount; i++)
        {
            GameObject ballInstance = Instantiate(ballPrefab, transform.position, Quaternion.identity);

            ballInstance.GetComponent<BallController>().StartMoving();
        }
    }
}
