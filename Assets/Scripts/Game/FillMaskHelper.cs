using UnityEngine;
using UnityEngine.UI;

public class FillMaskHelper : MonoBehaviour
{
    [SerializeField] private Image _FillMask;
    public Image GetFillMask() 
    {
        if (_FillMask == null) { Debug.Log($"Fillmask for {gameObject.name} is NULL"); }
        return _FillMask;
    }
}