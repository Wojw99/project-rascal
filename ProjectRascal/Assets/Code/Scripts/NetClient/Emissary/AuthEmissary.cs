using NetClient;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class AuthEmissary : MonoBehaviour
    {
        public string AuthToken = "gracz"; // in the future we have to create authorization service 
                                    // we have to store also other information in Token, like username, and by username we can receive data from Database to that client

        public delegate void AuthAction();

        public AuthAction OnLoginSucces;
        public AuthAction OnLoginFailed;

        #region Singleton

        private static AuthEmissary instance;

        public static AuthEmissary Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AuthEmissary>();
                    if (instance == null)
                        instance = new GameObject("AdventurerStateEmissary").AddComponent<AuthEmissary>();
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

        #endregion

        public async Task LoginToServer(string login, string password)
        {
            ClientSingleton Client = ClientSingleton.GetInstance();

            if (Client.AuthServer.IsConnected)
            {
                await Client.AuthServer.SendPacket(new ClientLoginRequestPacket(login, password));

                try
                {
                    PacketBase packet = await Client.WaitForResponsePacket(TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromSeconds(20), PacketType.LOGIN_RESPONSE);

                    if (packet is ClientLoginResponsePacket loginResponse)
                    {
                        AuthToken = loginResponse.AuthToken;
                        OnLoginSucces?.Invoke();
                    }
                    else
                    {
                        OnLoginFailed?.Invoke();
                    }
                }

                catch (TimeoutException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    Client.AuthServer.Disconnect();
                    OnLoginFailed?.Invoke();
                }
            }
            else
            {
                OnLoginFailed?.Invoke();
            }
        }
    }
}
