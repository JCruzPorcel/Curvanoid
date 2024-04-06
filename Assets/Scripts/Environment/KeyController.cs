using UnityEngine;

public class KeyController : MonoBehaviour
{
    public delegate void OnKeyAcquiredDelegate(bool OnKeyAcquiered);
    public static event OnKeyAcquiredDelegate OnKeyAcquired;

    [SerializeField] private float fallSpeed = 2f;

    private bool isFalling = false;

    private LevelController levelController;

    private void Start()
    {
        levelController = FindFirstObjectByType<LevelController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnKeyAcquired?.Invoke(true);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsCurrentState(GameManager.GameState.InGame)) return;

        if (isFalling)
        {
            Vector3 newPosition = transform.position + Vector3.down * fallSpeed * Time.fixedDeltaTime;
            transform.position = newPosition;

            if (transform.position.y < ResolutionManager.BottomBoundary)
            {
                OnKeyAcquired?.Invoke(false);
                isFalling = false;
                gameObject.SetActive(false);
            }
        }
        if (levelController != null && levelController.RemainingBricks == 1)
        {
            isFalling = true;
        }
    }
}
