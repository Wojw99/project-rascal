/*using MemoryPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkData
{
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

    [MemoryPackable(GenerateType.NoGenerate)]
    [MemoryPackUnion(0, typeof(PlayerAttributes))]
    [MemoryPackUnion(1, typeof(PlayerTransform))]
    public partial interface IGamePacket
    {
    }

    // AssemblyB define definition outside of target type
*//*    [MemoryPackUnionFormatter(typeof(IGamePacket))]
    [MemoryPackUnion(0, typeof(PlayerAttributes))]
    [MemoryPackUnion(1, typeof(PlayerTransform))]
    public partial class UnionSampleFormatter
    {
    }*/


    /*[MemoryPackable]
    [MemoryPackUnion(0, typeof(PlayerTransform))]
    [MemoryPackUnion(1, typeof(PlayerAttributes))]
    public abstract partial class GamePacket
    {
        public int Id { get; set; }
        public int PacketSize { get; set; }
        public MessageType _MessageType { get; set; }
        public Type RequestedType { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public TimeSpan ResponseTimeout { get; set; }
        public bool IsEncrypted { get; set; }
        public bool RequiresAcknowledgment { get; set; }

        public bool Encrypt { get; set; }

 *//*       [MemoryPackOnSerializing]
        void OnSerializing2()
        {
            if (Encrypt)
            {

            }
        }*//*

        public GamePacket()
        {
            Id = -1;
            PacketSize = 0;
            _MessageType = MessageType.message;
            ErrorMessage = string.Empty;
            TimeStamp = DateTime.UtcNow;
            ResponseTimeout = TimeSpan.FromMilliseconds(20);
            IsEncrypted = false;
            RequiresAcknowledgment = false;
        }
    }*//*
}
*/