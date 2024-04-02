using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset inputActions;

    private void OnEnable()
    {
        LoadBindings();
    }

    private void OnDisable()
    {
        SaveBindings();
    }

    public void LoadBindings()
    {
        var savedBindings = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(savedBindings))
        {
            inputActions.LoadBindingOverridesFromJson(savedBindings);
        }
    }

    public void SaveBindings()
    {
        var bindingsJson = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", bindingsJson);
    }
}
