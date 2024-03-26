using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Margen en el eje X para la Curva de Bezier")]
    private float additionalXMargin = 1f; // Margen adicional en X

    [SerializeField]
    [Tooltip("Margen en el eje Y para la Curva de Bezier")]
    private float additionalYMargin = 5f; // Margen adicional en Y

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private Vector3 controlPoint1;
    [SerializeField] private Vector3 controlPoint2;
    private int segmentsNumber = 20;

    private void Start()
    {
        ResolutionManager.OnResolutionChanged += UpdateCurve;
        UpdateCurve(new Vector2(Screen.width, Screen.height));
    }

    private void UpdateCurve(Vector2 resolution)
    {
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float xOffset = (cameraWidth * 0.5f) + -additionalXMargin;
        float yOffset = (cameraHeight * 0.5f) + additionalYMargin;

        startPoint = new Vector3(-xOffset, -2f, 0f);
        endPoint = new Vector3(xOffset, -2f, 0f);
        controlPoint1 = new Vector3(-xOffset + 2.5f, -yOffset, 0f);
        controlPoint2 = new Vector3(xOffset - 2.5f, -yOffset, 0f);
    }

    private void OnDrawGizmos()
    {
        if (startPoint == null || controlPoint1 == null || controlPoint2 == null || endPoint == null)
            return;

        Vector3 previousPoint = startPoint;

        for (int i = 1; i <= segmentsNumber; i++)
        {
            float parameter = i / (float)segmentsNumber;
            Vector3 point = GetPointSlerp(startPoint, controlPoint1, controlPoint2, endPoint, parameter);
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }

    public Vector3 GetPoint(float t)
    {
        return GetPointSlerp(startPoint, controlPoint1, controlPoint2, endPoint, t);
    }

    private Vector3 GetPointSlerp(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
               3f * oneMinusT * oneMinusT * t * p1 +
               3f * oneMinusT * t * t * p2 +
               t * t * t * p3;
    }

    public Vector3 GetTangent(float t)
    {
        t = Mathf.Clamp01(t);

        float oneMinusT = 1f - t;
        Vector3 tangent = 3f * oneMinusT * oneMinusT * (controlPoint1 - startPoint) +
                          6f * oneMinusT * t * (controlPoint2 - controlPoint1) +
                          3f * t * t * (endPoint - controlPoint2);

        return tangent.normalized; // Normalizamos el vector tangente para obtener la dirección sin afectar la magnitud.
    }
}
