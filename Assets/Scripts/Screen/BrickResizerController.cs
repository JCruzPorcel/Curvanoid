using UnityEngine;

public class BrickResizerController : MonoBehaviour
{
    [SerializeField] private Vector2 anchorOffsetPercentage = new Vector2(0.5f, 0.5f); // Porcentaje de ajuste desde el centro
    [SerializeField] private Vector2 anchorOffsetPixels = Vector2.zero; // Offset adicional en píxeles

    private Vector3 initialPosition;
    private Camera mainCamera;

    private void Start()
    {
        // Guardar la posición inicial del objeto
        initialPosition = transform.position;

        // Obtener la cámara principal
        mainCamera = Camera.main;

        // Actualizar la posición del objeto basándose en la resolución actual
        UpdateAnchorPosition(Vector2.zero);
    }

    private void OnEnable()
    {
        // Suscribirse al evento de cambio de resolución
        ResolutionManager.OnResolutionChanged += UpdateAnchorPosition;
    }

    private void OnDisable()
    {
        // Desuscribirse del evento de cambio de resolución
        ResolutionManager.OnResolutionChanged -= UpdateAnchorPosition;
    }

    private void UpdateAnchorPosition(Vector2 resolution)
    {
        if (mainCamera == null) return;

        // Obtener el tamaño de la pantalla en unidades de mundo
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Calcular la posición del anclaje en la pantalla
        float anchorX = screenWidth * anchorOffsetPercentage.x + anchorOffsetPixels.x;
        float anchorY = screenHeight * anchorOffsetPercentage.y + anchorOffsetPixels.y;

        // Convertir la posición del anclaje a coordenadas del mundo
        Vector3 anchorPosition = mainCamera.ScreenToWorldPoint(new Vector3(anchorX, anchorY, mainCamera.nearClipPlane));

        // Mantener el eje Z constante en la posición inicial
        anchorPosition.z = initialPosition.z;

        // Establecer la posición del objeto al anclaje calculado
        transform.position = anchorPosition;
    }
}
