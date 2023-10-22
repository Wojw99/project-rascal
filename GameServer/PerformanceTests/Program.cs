using System;
using System.Diagnostics;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkUtility;
using NetworkCore.Packets;
//using BinaryPack.Attributes;
//using BinaryPack;
//using BinaryPack.Enums;
using System.Reflection;
using NetworkCore.NetworkMessage;
using MemoryPack;

[MemoryPackable]
public partial class ClientLoginRequestPacketFromLib
{
    public string Login { get; set; }

    public string Password { get; set; }

    public ClientLoginRequestPacketFromLib(string login, string password) 
    { 
        Login = login;
        Password = password;
    }

}

[MemoryPackable]
public partial class ClientLoginRequestCollectionPacketFromLib
{
    public List<ClientLoginRequestPacketFromLib> PacketList { get; set; }

    public ClientLoginRequestCollectionPacketFromLib()
    {
        PacketList = new List<ClientLoginRequestPacketFromLib>();
    }

}

public class ClientLoginRequestPacket : PacketBase
{
    [Serialization(Type: SerializationType.type_string)]
    public string Login { get; private set; }

    [Serialization(Type: SerializationType.type_string)]
    public string Password { get; private set; }

    public ClientLoginRequestPacket(string login, string password) : base(PacketType.LOGIN_REQUEST, false)
    {
        Login = login;
        Password = password;
    }

    public ClientLoginRequestPacket(byte[] data) : base(data) { }
    public override string GetInfo()
    {
        return "LOGIN REQUEST PACKET, " + base.GetInfo();
    }
}

public class ClientLoginRequestCollectionPacket : PacketBase
{
    [Serialization(Type: SerializationType.type_subPacketList)]
    public List <ClientLoginRequestPacket> PacketList { get; private set; }

    public ClientLoginRequestCollectionPacket() : base(PacketType.LOGIN_REQUEST_COLLECTION, false)
    {
        PacketList = new List<ClientLoginRequestPacket>();
    }

    public ClientLoginRequestCollectionPacket(byte[] data) : base(data) { }
    public override string GetInfo()
    {
        return "LOGIN REQUEST PACKET, " + base.GetInfo();
    }
}

namespace PerformanceTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            
            ClientLoginRequestPacket MyLoginReq = new ClientLoginRequestPacket("login", "password");
            ClientLoginRequestPacketFromLib HisLoginReq = new ClientLoginRequestPacketFromLib("login", "password");

            stopwatch.Start();
            byte[] data = MyLoginReq.Serialize();
            stopwatch.Stop();
            Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            PacketBase deserializedPacket = new ClientLoginRequestPacket(data);
            if(deserializedPacket is ClientLoginRequestPacket request)
            {

            }
            stopwatch.Stop();
            Console.WriteLine($"Mój czas deserializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            var data2 = MemoryPackSerializer.Serialize(HisLoginReq);
            stopwatch.Stop();
            Console.WriteLine($"Czas serializacji biblioteki: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            var loaded = MemoryPackSerializer.Deserialize<ClientLoginRequestPacketFromLib>(data2);
            stopwatch.Stop();
            Console.WriteLine($"Czas deserializacji biblioteki: {stopwatch.Elapsed.TotalMilliseconds} ms");


            ClientLoginRequestCollectionPacket myPacket = new ClientLoginRequestCollectionPacket();
            myPacket.PacketList.Add(new ClientLoginRequestPacket("login1", "password1"));
            myPacket.PacketList.Add(new ClientLoginRequestPacket("login2", "password2"));
            myPacket.PacketList.Add(new ClientLoginRequestPacket("login3", "password3"));
            myPacket.PacketList.Add(new ClientLoginRequestPacket("login4", "password4"));

            ClientLoginRequestCollectionPacketFromLib hisPacket = new ClientLoginRequestCollectionPacketFromLib();
            hisPacket.PacketList.Add(new ClientLoginRequestPacketFromLib("login1", "password1"));
            hisPacket.PacketList.Add(new ClientLoginRequestPacketFromLib("login2", "password2"));
            hisPacket.PacketList.Add(new ClientLoginRequestPacketFromLib("login3", "password3"));
            hisPacket.PacketList.Add(new ClientLoginRequestPacketFromLib("login4", "password4"));


            /*stopwatch.Start();
            byte[] myData = myPacket.Serialize();
            stopwatch.Stop();
            Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            PacketBase deserializedPacket = new ClientLoginRequestCollectionPacket(myData);
            if (deserializedPacket is ClientLoginRequestCollectionPacket request)
            {

            }

            stopwatch.Reset();*/
/*
            stopwatch.Start();
            var hisData = MemoryPackSerializer.Serialize(hisPacket);
            stopwatch.Stop();
            Console.WriteLine($"Czas serializacji biblioteki: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            var loaded = MemoryPackSerializer.Deserialize<ClientLoginRequestPacketFromLib>(hisData);
            stopwatch.Stop();
            Console.WriteLine($"Czas deserializacji biblioteki: {stopwatch.Elapsed.TotalMilliseconds} ms"); */

            /*  Server.StartListen();
              Server.StartPacketProcessing(50, 50, TimeSpan.FromMilliseconds(1));
              Server.StartUpdate(TimeSpan.FromMilliseconds(1));
              //await Task.Run(async () => await TestingOperationsTask(Server));

              while (true)
              {
                  Thread.Sleep(5000);
                  await Console.Out.WriteLineAsync("Serwer dziala...");
              }*/


        }
    }
}