using System;
using System.Diagnostics;
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
            //StandardPacket standardPacket = new StandardPacket();
            //standardPacket.Test = 12;

            //byte[] data = PacketSerializerType.Serialize(standardPacket);

            //StandardPacket packet = PacketSerializerType.Deserialize<StandardPacket>(data);

            Stopwatch stopwatch = new Stopwatch();
            double sumMethod1 = 0;
            double sumMethod2 = 0;
            int count = 1;
            //Thread.Sleep(5000);

            TestPacket packet = new TestPacket();
            TestPacket2 packet2 = new TestPacket2();
            byte[] data;
            byte[] data2;

            for (int i = 0; i < count; i++)
            {
                //TransformPacket packet = new TransformPacket(1, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
                

                //Console.WriteLine("SPOSOB 1 (TransformPacket)");

                stopwatch.Start();
                data = PacketSerializer.Serialize(packet);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumMethod1 += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();

                stopwatch.Start();
                var desPacket = PacketSerializer.Deserialize<TestPacket>(data);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas deserializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumMethod1 += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();

                //******************************************************************************************

                //TransformPacket2 packet2 = new TransformPacket2(1, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
                

                //Console.WriteLine("SPOSOB 2 (TransformPacket2)");

            }

            Console.WriteLine($"Ilosc operacji = {count}");
            Console.WriteLine($"Suma metody 1 = {sumMethod1} ms, {sumMethod1/1000} s.");
            Console.WriteLine($"Suma metody 2 = {sumMethod2} ms, {sumMethod2/1000} s.");


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
            hisPacket.PacketList.Add(new ClientLoginRequestPacketFromLib("login4", "password4"));*/


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