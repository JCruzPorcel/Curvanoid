using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public delegate void ResolutionChangedDelegate(Vector2 resolution);
    public static event ResolutionChangedDelegate OnResolutionChanged;

    private void Start()
    {
        UpdateResolution();
    }

    private void UpdateResolution()
    {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        OnResolutionChanged?.Invoke(resolution);
    }

    private void Update()
    {
        if (Screen.width != Mathf.RoundToInt(transform.localScale.x) || Screen.height != Mathf.RoundToInt(transform.localScale.y))
        {
            UpdateResolution();
        }
    }
}
