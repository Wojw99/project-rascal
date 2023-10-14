using Assets.Code.Scripts.NetClient.Emissary;
using NetClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Base
{
    public class StartSingleton : MonoBehaviour
    {
        public static StartSingleton instance;
        
        private void Awake()
        {
            instance = this;
// ClientSingleton client = ClientSingleton.GetInstanceAsync();
        }
    }
}
