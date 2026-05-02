using UnityEngine.Networking;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _Instance;
    public static NetworkManager Instance;

    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #region Player Connection

    //OnPlayerConnect

    //OnPlayerDisconnect

    //MakeHost

    //AssignPlayerSlot

    #endregion
}
