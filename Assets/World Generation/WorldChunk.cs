using NaughtyAttributes;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class WorldChunk : MonoBehaviour 
{
    public event System.Action<bool> OnVisibilityChanged;

    private void Awake()
    {
        SetVisible(IsVisible);
    }

    [ShowNonSerializedField, ReadOnly]
    private bool isVisible;
    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (isVisible != value)
                SetVisible(value);
        }
    }

    private void SetVisible(bool value)
    {
        isVisible = value;
        if (isVisible && gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            OnVisibilityChanged?.Invoke(true);
        }
    }

    private void LateUpdate()
    {
        if (isVisible == false)
        {
            gameObject.SetActive(false);
            OnVisibilityChanged?.Invoke(false);
        }
    }
}
