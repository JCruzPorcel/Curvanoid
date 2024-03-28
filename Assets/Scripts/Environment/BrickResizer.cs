using UnityEngine;

public class BrickResizer : MonoBehaviour
{
    [SerializeField] private float widthPercentage = 0.1f; // Porcentaje del ancho de la pantalla que ocupará un ladrillo
    [SerializeField] private float heightPercentage = 0.05f; // Porcentaje de la altura de la pantalla que ocupará un ladrillo
    [SerializeField] private LayerMask brickLayer; // Capa de los ladrillos

    private void OnEnable()
    {
        // Ajusta el tamaño de los ladrillos al iniciar el juego
        ResizeBricks();

        // Suscribirse al evento OnResolutionChanged de ResolutionManager
        ResolutionManager.OnResolutionChanged += OnResolutionChangedHandler;
    }

    private void OnDisable()
    {
        // Asegúrate de desuscribirte del evento cuando el objeto sea destruido
        ResolutionManager.OnResolutionChanged -= OnResolutionChangedHandler;
    }

    private void OnResolutionChangedHandler(Vector2 resolution)
    {
        // Reajustar el tamaño de los ladrillos cuando cambie la resolución de la pantalla
        ResizeBricks();
    }

    private void ResizeBricks()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float brickWidth = screenWidth * widthPercentage;
        float brickHeight = screenHeight * heightPercentage;

        // Encontrar todos los objetos en la capa de ladrillos
        Collider2D[] bricks = Physics2D.OverlapAreaAll(Vector2.zero, new Vector2(screenWidth, screenHeight), brickLayer);

        foreach (Collider2D brickCollider in bricks)
        {
            // Obtener el componente SpriteRenderer del ladrillo
            SpriteRenderer brickRenderer = brickCollider.GetComponent<SpriteRenderer>();
            if (brickRenderer != null)
            {
                // Establecer el tamaño del ladrillo
                brickRenderer.size = new Vector2(brickWidth, brickHeight);
            }
        }
    }
}
