using NetworkCore.NetworkMessage;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using Assets.Code.Scripts.NetClient;
using System;

public class Client : MonoBehaviour
{
    private static Client _instance;
    public static ClientNetwork _instanceNetwork = new ClientNetwork(100, 100, System.TimeSpan.FromMilliseconds(50));
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

    private async void Start()
    {
        await _instanceNetwork.ConnectTcpServer("192.168.5.2", 8051);
    }
}
