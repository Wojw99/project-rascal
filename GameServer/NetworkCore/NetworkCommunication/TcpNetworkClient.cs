using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public abstract class TcpNetworkClient
    {
        public PacketHandler _PacketHandler { get; private set; }
        public PacketSender _PacketSender { get; private set; }

        // SerializeField in this case doesn't work
        protected bool IsRunning { get; private set; }
        protected TimeSpan UpdateDelay { get; private set; }

        public TcpNetworkClient()
        {
            _PacketHandler = new PacketHandler();
            _PacketSender = new PacketSender();
            IsRunning = true;
            UpdateDelay = TimeSpan.FromMilliseconds(5);
        }
        public void StartUpdate()
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
