using UnityEngine;

public class WallResizer : MonoBehaviour
{
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform topWall;
    [SerializeField] private Transform bottomWall;
    [SerializeField] private float sideWallInclination = 30f; // Inclinación de las paredes laterales en grados
    [SerializeField] private float bottomSpace = 1f; // Espacio en la parte inferior de la pantalla

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        AdjustWalls();
    }

    private void Update()
    {
        AdjustWalls();
    }

    private void AdjustWalls()
    {
        // Obtener las dimensiones de la cámara
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calcular la posición de la pared inferior con espacio adicional
        float bottomY = mainCamera.transform.position.y - mainCamera.orthographicSize + bottomSpace;

        // Posicionar las paredes
        leftWall.position = new Vector3(mainCamera.transform.position.x - cameraWidth / 2f, mainCamera.transform.position.y, leftWall.position.z);
        rightWall.position = new Vector3(mainCamera.transform.position.x + cameraWidth / 2f, mainCamera.transform.position.y, rightWall.position.z);
        topWall.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + cameraHeight / 2f, topWall.position.z);
        bottomWall.position = new Vector3(mainCamera.transform.position.x, bottomY, bottomWall.position.z);

        // Escalar las paredes
        leftWall.localScale = new Vector3(leftWall.localScale.x, cameraHeight, leftWall.localScale.z);
        rightWall.localScale = new Vector3(rightWall.localScale.x, cameraHeight, rightWall.localScale.z);
        topWall.localScale = new Vector3(cameraWidth, topWall.localScale.y, topWall.localScale.z);
        bottomWall.localScale = new Vector3(cameraWidth, bottomWall.localScale.y, bottomWall.localScale.z);

        // Inclinar las paredes laterales
        float sideWallYScale = cameraHeight + bottomSpace * 2f; // Ajuste de altura para incluir el espacio en la parte inferior
        float sideWallXScale = Mathf.Abs(bottomWall.position.y - topWall.position.y) / Mathf.Tan(Mathf.Deg2Rad * sideWallInclination);
        leftWall.localScale = new Vector3(sideWallXScale, -sideWallYScale, leftWall.localScale.z);
        rightWall.localScale = new Vector3(sideWallXScale, sideWallYScale, rightWall.localScale.z);

        // Rotar las paredes laterales
        leftWall.rotation = Quaternion.Euler(0f, 0f, sideWallInclination);
        rightWall.rotation = Quaternion.Euler(0f, 0f, -sideWallInclination);
    }
}
