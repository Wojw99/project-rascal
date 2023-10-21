using Assets.Code.Scripts.NetClient.Emissary;
using NetClient;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Base
{
    public class StartSingleton : MonoBehaviour
    {
        #region Singleton

        private static StartSingleton instance;

        public static StartSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<StartSingleton>();
                    if (instance == null)
                        instance = new GameObject("AdventurerStateEmissary").AddComponent<StartSingleton>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null) {
                instance = this;
                ClientSingleton.GetInstance().GameServer.ConnectToServer();
            }
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        #endregion

        /*private IEnumerator StartServers()
        {
            yield return UnityTaskUtils.RunTaskAsync(() => ClientSingleton.GetInstance().GameServer.ConnectToServer());
            //yield return UnityTaskUtils.RunTaskAsync(() => ClientSingleton.GetInstance().AuthServer.ConnectToServer());
        }*/
    }
}
