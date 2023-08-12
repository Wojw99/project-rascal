using NetworkCore.NetworkMessage;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class Client : MonoBehaviour
{
    private static Client _instance;
    public static ClientNetworkBase _instanceNetwork = new ClientNetworkBase();
    public static Client Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Client>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Client>();
                    singletonObject.name = "ClientSingleton";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        yield return _instanceNetwork.ConnectTcpServer("127.0.0.1", 8051);
        yield return _instanceNetwork.Start();
    }
}
