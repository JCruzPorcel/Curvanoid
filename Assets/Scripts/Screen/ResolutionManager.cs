using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public delegate void ResolutionChangedDelegate(Vector2 resolution);
    public static event ResolutionChangedDelegate OnResolutionChanged;

    // Variables para almacenar los límites de la pantalla
    private static float leftBoundary;
    private static float rightBoundary;
    private static float topBoundary;
    private static float bottomBoundary;

    private void Start()
    {
        UpdateResolution();
    }

    private void UpdateResolution()
    {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        CalculateScreenBoundaries(); // Calcular los límites de la pantalla
        OnResolutionChanged?.Invoke(resolution);
    }

    private void LateUpdate()
    {
        // Si la resolución cambia, actualizar los límites y emitir el evento
        if (Screen.width != Mathf.RoundToInt(transform.localScale.x) || Screen.height != Mathf.RoundToInt(transform.localScale.y))
        {
            UpdateResolution();
        }
    }

    // Método para calcular los límites de la pantalla
    private void CalculateScreenBoundaries()
    {
        // Convertir la posición de la cámara a coordenadas del mundo
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Almacenar los límites de la pantalla
        leftBoundary = bottomLeft.x;
        rightBoundary = topRight.x;
        topBoundary = topRight.y;
        bottomBoundary = bottomLeft.y;
    }

    // Propiedades para acceder a los límites de la pantalla
    public static float LeftBoundary => leftBoundary;
    public static float RightBoundary => rightBoundary;
    public static float TopBoundary => topBoundary;
    public static float BottomBoundary => bottomBoundary;
}
