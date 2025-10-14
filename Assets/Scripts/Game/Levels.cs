using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    public Levels Instance { get { return _instance; }}
    private Levels _instance;

    Levels()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void LoadSceneInBackground(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
#endif        
        Application.Quit();

    }
}
