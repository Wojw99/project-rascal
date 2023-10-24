using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using PerformanceTests.Test;

namespace PerformanceTests
{
   // Last test on 10 million count:
   // Sum method 1 = 33147,57929850525 ms, 33,14757929850525 s.
   // Sum method 2 = 45320,16210527336 ms, 45,32016210527336 s.
    internal class Program
    {
        static void Main(string[] args)
        {
            MesaureSerializingTime(1000);

            Console.WriteLine("Tera pomiar:");

            MesaureSerializingTime(100000);
            MesaureSerializingTime2(100000);

            #region notUsed

            /*Stopwatch stopwatch = new Stopwatch();

            ClientLoginRequestPacket MyLoginReq = new ClientLoginRequestPacket("login", "password");
            ClientLoginRequestPacketFromLib HisLoginReq = new ClientLoginRequestPacketFromLib("login", "password");

            stopwatch.Start();
            byte[] data = MyLoginReq.Serialize();
            stopwatch.Stop();
            Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            PacketBase deserializedPacket = new ClientLoginRequestPacket(data);
            if (deserializedPacket is ClientLoginRequestPacket request)
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


            stopwatch.Start();
            byte[] myData = myPacket.Serialize();
            stopwatch.Stop();
            Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            PacketBase deserializedPacket = new ClientLoginRequestCollectionPacket(myData);
            if (deserializedPacket is ClientLoginRequestCollectionPacket request)
            {

            }

            stopwatch.Reset();

            stopwatch.Start();
            var hisData = MemoryPackSerializer.Serialize(hisPacket);
            stopwatch.Stop();
            Console.WriteLine($"Czas serializacji biblioteki: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Reset();

            stopwatch.Start();
            var loaded = MemoryPackSerializer.Deserialize<ClientLoginRequestPacketFromLib>(hisData);
            stopwatch.Stop();
            Console.WriteLine($"Czas deserializacji biblioteki: {stopwatch.Elapsed.TotalMilliseconds} ms");

            Server.StartListen();
            Server.StartPacketProcessing(50, 50, TimeSpan.FromMilliseconds(1));
            Server.StartUpdate(TimeSpan.FromMilliseconds(1));
            //await Task.Run(async () => await TestingOperationsTask(Server));

            while (true)
            {
                Thread.Sleep(5000);
                await Console.Out.WriteLineAsync("Serwer dziala...");
            }*/

            #endregion notUsed
        }

        private static void MesaureSerializingTime(int count)
        {
            Stopwatch stopwatch = new Stopwatch();
            double sumSerialization = 0;
            double sumDeserialization = 0;
            Thread.Sleep(1000);

            TestPacket packet = new TestPacket();
            byte[] data;

            for (int i = 0; i < count; i++)
            {
                stopwatch.Start();
                data = PacketSerializer.Serialize(packet);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumSerialization += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();

                stopwatch.Start();
                var desPacket = PacketSerializer.Deserialize<TestPacket>(data);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas deserializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumDeserialization += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();
            }
            Console.WriteLine("TEST PACKET: ");
            Console.WriteLine($"Ilosc operacji = {count}");
            Console.WriteLine($"Suma serializacja = {sumSerialization} ms, {sumSerialization / 1000} s.");
            Console.WriteLine($"Suma deserializacja = {sumDeserialization} ms, {sumDeserialization / 1000} s.");
        }

        private static void MesaureSerializingTime2(int count)
        {
            Stopwatch stopwatch = new Stopwatch();
            double sumSerialization = 0;
            double sumDeserialization = 0;
            Thread.Sleep(1000);

            SimpleTestPacket packet = new SimpleTestPacket();
            byte[] data;

            for (int i = 0; i < count; i++)
            {
                stopwatch.Start();
                data = PacketSerializer.Serialize(packet);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumSerialization += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();

                stopwatch.Start();
                var desPacket = PacketSerializer.Deserialize<SimpleTestPacket>(data);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas deserializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumDeserialization += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();
            }

            Console.WriteLine("SIMPLE TEST PACKET: ");
            Console.WriteLine($"Ilosc operacji = {count}");
            Console.WriteLine($"Suma serializacja = {sumSerialization} ms, {sumSerialization / 1000} s.");
            Console.WriteLine($"Suma deserializacja = {sumDeserialization} ms, {sumDeserialization / 1000} s.");
        }
    }
}