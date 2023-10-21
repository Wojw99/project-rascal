using NetClient;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections;
using UnityEngine;
//using Assets.Code.Scripts.NetClient.Base;
using System.Threading.Tasks;
using Assets.Code.Scripts.NetClient.Emissary;
using Assets.Code.Scripts.NetClient;

public class NetworkTimeSyncEmissary : MonoBehaviour
{
    [SerializeField] private float SyncFrequency = 5f;
    private float lastSyncTime = 0f;
    private float NetworkLatency = 0f;

    #region Singleton

    private static NetworkTimeSyncEmissary instance;

    public static NetworkTimeSyncEmissary Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetworkTimeSyncEmissary>();
                if (instance == null)
                    instance = new GameObject("NetworkTimeSyncEmissary").AddComponent<NetworkTimeSyncEmissary>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lastSyncTime = Time.time;
        //StartCoroutine(SendPingToServer());
    }

    /*private void Update()
    {
        
    }*/

    #endregion

    public float GetNetworkLatency()
    {
        lock(this)
        {
            return NetworkLatency;
        }
    }

    private IEnumerator SendPingToServer()
    {
        ClientSingleton client = ClientSingleton.GetInstance();

        float startTime = Time.time;

        yield return UnityTaskUtils.RunTaskAsync(async () => await client.GameServer.SendPacket(new PingRequestPacket()));

        float pingEndTime = Time.time;

        // Oblicz czas trwania pinga
        float pingDuration = (pingEndTime - startTime) * 1000;
        Debug.Log($"Ping to server: {pingDuration} ms");

        yield return UnityTaskUtils.RunTaskAsync(async () =>
        {
            try
            {
                await client._PacketHandler.WaitForResponsePacket(client.GameServer.GUID, PacketType.PING_RESPONSE);
            }
            catch (TimeoutException ex)
            {
                Debug.LogWarning(ex);
            }
        });

        float pongEndTime = Time.time;
        float pingPongDuration = (pongEndTime - startTime) * 1000;
        Debug.Log($"Ping-pong to server: {pingPongDuration} ms");

        // Oblicz network latency jako połowę czasu trwania ping-ponga
        float networkLatency = pingPongDuration / 2;
        Debug.Log($"Network Latency: {networkLatency} ms");

        // Aktualizuj network latency w swojej klasie
        lock (this)
        {
            NetworkLatency = networkLatency;
        }

        // Odczekaj przed wysłaniem kolejnego pinga (dostosuj częstotliwość)
        yield return new WaitForSeconds(SyncFrequency);

        // Ponownie uruchom korutynę
        StartCoroutine(SendPingToServer());
    }

    /*    public void StartSynchronize()
        {
            StartCoroutine(SynchronizeTime());
        }*/

    /*private IEnumerator SynchronizeTime()
    {
        // Wysyłanie pinga
        float startTime = Time.time;

        yield return StartCoroutine(SendPingToServer());

        // Otrzymywanie ponga
        float endTime = Time.time;
        float roundTripTime = endTime - startTime;
        float oneWayTime = roundTripTime / 2f;

        // Obliczanie opóźnienia
        this.NetworkLatency = oneWayTime;

        // Odczekaj przed wysłaniem kolejnego pinga (dostosuj częstotliwość)
        yield return new WaitForSeconds(SyncFrequency); // Przykładowa częstotliwość, dostosuj do potrzeb
    }

    private IEnumerator SendPingToServer()
    {
        ClientSingleton client = ClientSingleton.GetInstance();

        float startTime = Time.time;

        yield return UnityTaskUtils.RunTaskAsync(async () => await client.GameServer.SendPacket(new PingRequestPacket()));

        float pingEndTime = Time.time;

        Debug.Log($"Ping to server: {(pingEndTime - startTime) * 1000 } ms.");

        yield return UnityTaskUtils.RunTaskAsync(async () =>
        {
            try
            {
                await client._PacketHandler.WaitForResponsePacket(client.GameServer.GUID, PacketType.PING_RESPONSE);
            }
            catch (TimeoutException ex)
            {
                Debug.LogWarning(ex);
            }
        });


        float pongEndTime = Time.time;
        Debug.Log($"Ping-pong to server: {(pongEndTime - startTime) * 1000 } ms.");
    }*/

    /*    private async Task <float> GetServerTime()
        {
            // Pobranie czasu serwera
            // ... implementacja pobierania czasu serwera ...
            return 0f; // Zastąp prawdziwą wartością czasu serwera
        }

        private void UpdateGameTime(float localTime)
        {
            // Aktualizacja lokalnego zegara gry
            Time.timeScale = 1f; // Możesz dostosować czas przyspieszenia/zwalniania gry w zależności od potrzeb
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // Dostosuj fixedDeltaTime w zależności od przyspieszenia/zwalniania
            Time.time = localTime; // Ustaw czas gry na czas lokalny
        }*/

}
