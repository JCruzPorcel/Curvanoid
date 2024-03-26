using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform startPoint;
    public Transform controlPoint1;
    public Transform controlPoint2;
    public Transform endPoint;
    [HideInInspector] public int segmentsNumber = 20;

    private void OnDrawGizmos()
    {
        // Ensure that all points are assigned
        if (startPoint == null || controlPoint1 == null || controlPoint2 == null || endPoint == null)
            return;

        Vector3 previousPoint = startPoint.position;

        for (int i = 1; i <= segmentsNumber; i++)
        {
            float parameter = i / (float)segmentsNumber;
            Vector3 point = GetPoint(startPoint.position, controlPoint1.position, controlPoint2.position, endPoint.position, parameter);
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }

    public Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
               3f * oneMinusT * oneMinusT * t * p1 +
               3f * oneMinusT * t * t * p2 +
               t * t * t * p3;
    }
}