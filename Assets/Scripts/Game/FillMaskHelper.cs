using UnityEngine;
using UnityEngine.UI;

public class FillMaskHelper : MonoBehaviour
{
    [SerializeField] private Image _FillMask;
    public Image GetFillMask() {  return _FillMask; }

    private void OnEnable()
    {
        _FillMask.fillAmount = 0.0f;
    }
}