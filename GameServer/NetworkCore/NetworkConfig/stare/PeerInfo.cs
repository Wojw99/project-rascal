using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetworkCore.NetworkConfig.stare
{
    public class PeerInfo
    {
        public IPEndPoint MasterEndPoint { get; set; }
        public int ConnectRetryIntervalSeconds { get; set; }
        public int MaxConnTries { get; set; }
        public int NumConnTries { get; set; }

        public PeerInfo(string adress, int port, int connectRetryIntervalSeconds, int maxConnTries)
        {
            MasterEndPoint = new IPEndPoint(IPAddress.Parse(adress), port);
            ConnectRetryIntervalSeconds = connectRetryIntervalSeconds;
            MaxConnTries = maxConnTries;
            NumConnTries = 0;
        }
    }
}
