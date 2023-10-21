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
    private float NetworkLatency;

    #region Singleton

    private static NetworkTimeSyncEmissary instance;

    public static NetworkTimeSyncEmissary Instance
    {
        get
        {
            if (instance == null)
                if (FindObjectOfType<NetworkTimeSyncEmissary>() == null)
                    instance = new GameObject("AdventurerStateEmissary").AddComponent<NetworkTimeSyncEmissary>();
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

    #endregion

    public float GetNetworkLatency()
    {
        lock(this)
        {
            return NetworkLatency;
        }
    }

    private IEnumerator SynchronizeTime()
    {
        while (true)
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
                await client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20),
                    TimeSpan.FromSeconds(50), PacketType.PING_RESPONSE);
            }
            catch (TimeoutException ex)
            {
                Debug.LogWarning(ex);
            }
        });


        float pongEndTime = Time.time;
        Debug.Log($"Ping-pong to server: {(pongEndTime - startTime) * 1000 } ms.");
    }

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
