using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
    private GameObject _Parent;
    private Button _Button;

    private void Awake()
    {
        _Button = GetComponent<Button>();
    }

    public void SetParent(GameObject parent)
    {
        _Parent = parent;
    }

}
        