using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore
{

    /**
     * @brief ByteBuffer class.
     * 
     * Currently work in progress. There are missing a few methods, but 
     * available methods work corecttly.
     *
     */
    public class ByteBuffer
    {
        private List<byte> _buffer;
        private byte [] _readBuff;
        private int _readPos;
        private bool _buffUpdated = false;


        public ByteBuffer ()
        {
            _buffer = new List<byte> ();
            //readBuff = new byte[1024];
            _readPos = 0;
        }

        public long GetReadPos()
        {
            return _readPos;
        }

        public byte[] ToArray()
        {
            return _buffer.ToArray();
        }

        public int Count()
        {
            return _buffer.Count;
        }

        public int Length()
        {
            return _buffer.Count - _readPos;
        }

        public void Clear()
        {
            _buffer.Clear();
            _readPos = 0;
        }

        public void WriteBytes(byte[] bytes)
        {
            _buffer.AddRange(bytes);
            
            _buffUpdated = true;
        }

        public void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
            _buffUpdated = true;
        }

        public void WriteInt(int value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
            _buffUpdated = true;
        }

        public void WriteLong(long value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
            _buffUpdated = true;
        }

        public void WriteInt(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
            _buffUpdated = true;
        }

        public void WriteString(string value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value.Length));
            _buffer.AddRange(Encoding.ASCII.GetBytes(value));
            _buffUpdated = true;
        }

        
        public int ReadInt(bool peek = true) 
        {
            if(_buffer.Count > _readPos)
            {
                if(_buffUpdated)
                {
                    _readBuff = _buffer.ToArray();
                }
            }
            int ret = BitConverter.ToInt32(_readBuff, _readPos);
            return ret;
        }

/*        public byte[] ReadBytes(int length, bool peek = true)
        {

        }*/

    }
}
