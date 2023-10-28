using MemoryPack;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using NetworkCore;
using NetworkCore.NetworkData;
using NetworkCore.NetworkCommunication;

public enum MessageType : byte
{
    message = 0x1,
    request = 0x2,
    response = 0x3
}

public enum EncryptionType : byte
{
    AES = 0x1
}


namespace MemoryPackTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            /*GamePacket sample = new PlayerTransform() { CharacterVId = 1, PosX = 12.3f, 
                PosY = 12.1f, PosZ = 12.4f, RotX = 11.2f, RotY = 11.8f, RotZ = 8.4f};
            
            var bin = MemoryPackSerializer.Serialize(sample);
            var reData = MemoryPackSerializer.Deserialize<GamePacket>(bin);
            if(reData is PlayerTransform x) 
            {
                Console.WriteLine($"CharacterVId = {x.CharacterVId}.");
                Console.WriteLine($"PosX = {x.PosX}.");
                Console.WriteLine($"PosX = {x.PosY}.");
                Console.WriteLine($"PosX = {x.PosZ}.");
                Console.WriteLine($"PosX = {x.RotX}.");
                Console.WriteLine($"PosX = {x.RotY}.");
                Console.WriteLine($"PosX = {x.RotZ}.");
            }*/
            
            
            PlayerAttributes attr = new PlayerAttributes();

            attr.CurrentHealth = 12;

            var data = MemoryPackSerializer.Serialize(attr);

            var deserialized = MemoryPackSerializer.Deserialize<PlayerAttributes>(data);

            Console.WriteLine("asdas");

            //MesaureSerializingTime(100);

            //MesaureSerializingTime(1000000);

            Console.WriteLine("Hello, World!");
        }

        private static void MesaureSerializingTime(int count)
        {
            // 
            Stopwatch stopwatch = new Stopwatch();
            double sumSerialization = 0;
            double sumDeserialization = 0;

            TransformCollection transformCollection = new TransformCollection();
            

            transformCollection.Add(
                new PlayerTransform() {
                CharacterVId = 1,
                PosX = 12.3f,
                PosY = 12.1f,
                PosZ = 12.4f,
                RotX = 11.2f,
                RotY = 11.8f,
                RotZ = 8.4f
                });

            Thread.Sleep(5000);

            byte[] data;

            for (int i = 0; i < count; i++)
            {

                stopwatch.Start();
                data = MemoryPackSerializer.Serialize(transformCollection);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas serializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumSerialization += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();

                stopwatch.Start();
                var desPacket = MemoryPackSerializer.Deserialize<TransformCollection>(data);
                stopwatch.Stop();
                //Console.WriteLine($"Mój czas deserializacji: {stopwatch.Elapsed.TotalMilliseconds} ms");
                sumDeserialization += (double)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();
            }

            Console.WriteLine("TEST PACKET (EXT LIBRARY): ");
            Console.WriteLine($"Ilosc operacji = {count}");
            Console.WriteLine($"Suma serializacja = {sumSerialization} ms, {sumSerialization / 1000} s.");
            Console.WriteLine($"Suma deserializacja = {sumDeserialization} ms, {sumDeserialization / 1000} s.");
        }
    }
}