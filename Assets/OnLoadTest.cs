using UnityEngine;
using UnityEngine.SceneManagement;

public class OnLoadTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Scenes open: " + SceneManager.sceneCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
