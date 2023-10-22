// Move that class to NetworkCore project.

using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Concurrent;
using NetworkCore.NetworkCommunication;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace NetClient
{
    public abstract class TcpNetworkClient 
    {
        public PacketHandler _PacketHandler { get; private set; }

        // SerializeField in this case doesn't work
        [SerializeField] protected bool IsRunning { get; private set; }
        [SerializeField] protected TimeSpan UpdateDelay { get; private set; }

        public TcpNetworkClient() 
        { 
            _PacketHandler = new PacketHandler();
            IsRunning = true;
            UpdateDelay = TimeSpan.FromMilliseconds(5);
            StartUpdate();
        }
        private void StartUpdate()
        {
            Task handleUpdate = Task.Run(async () =>
            {
                while (IsRunning)
                {
                    await Update();
                    await Task.Delay(UpdateDelay);
                }
            });
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public abstract Task Update();
    }
}
