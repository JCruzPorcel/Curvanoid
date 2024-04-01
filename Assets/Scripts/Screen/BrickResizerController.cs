using UnityEngine;

public class BrickResizerController : MonoBehaviour
{
    [SerializeField] private Vector2 anchorOffsetPercentage = new Vector2(0.5f, 0.5f); // Porcentaje de ajuste desde el centro
    [SerializeField] private Vector2 anchorOffsetPixels = Vector2.zero; // Offset adicional en p�xeles

    private Vector3 initialPosition;
    private Camera mainCamera;

    private void Start()
    {
        // Guardar la posici�n inicial del objeto
        initialPosition = transform.position;

        // Obtener la c�mara principal
        mainCamera = Camera.main;

        // Actualizar la posici�n del objeto bas�ndose en la resoluci�n actual
        UpdateAnchorPosition(Vector2.zero);
    }

    private void OnEnable()
    {
        // Suscribirse al evento de cambio de resoluci�n
        ResolutionManager.OnResolutionChanged += UpdateAnchorPosition;
    }

    private void OnDisable()
    {
        // Desuscribirse del evento de cambio de resoluci�n
        ResolutionManager.OnResolutionChanged -= UpdateAnchorPosition;
    }

    private void UpdateAnchorPosition(Vector2 resolution)
    {
        if (mainCamera == null) return;

        // Obtener el tama�o de la pantalla en unidades de mundo
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Calcular la posici�n del anclaje en la pantalla
        float anchorX = screenWidth * anchorOffsetPercentage.x + anchorOffsetPixels.x;
        float anchorY = screenHeight * anchorOffsetPercentage.y + anchorOffsetPixels.y;

        // Convertir la posici�n del anclaje a coordenadas del mundo
        Vector3 anchorPosition = mainCamera.ScreenToWorldPoint(new Vector3(anchorX, anchorY, mainCamera.nearClipPlane));

        // Mantener el eje Z constante en la posici�n inicial
        anchorPosition.z = initialPosition.z;

        // Establecer la posici�n del objeto al anclaje calculado
        transform.position = anchorPosition;
    }
}
