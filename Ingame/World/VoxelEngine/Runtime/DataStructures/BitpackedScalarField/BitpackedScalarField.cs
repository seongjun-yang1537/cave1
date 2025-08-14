using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using VoxelEngine;

namespace VoxelEngine
{
    [Serializable]
    public class BitpackedScalarField : IScalarField, ISerializationCallbackReceiver, IEnumerable<int>
    {
        [SerializeField]
        private int width, height, depth;
        public int Width => width;
        public int Height => height;
        public int Depth => depth;

        public Vector3Int Size => new Vector3Int(width, height, depth);

        [SerializeField]
        private byte[] data;
        public byte[] GetDataArray() => (byte[])data.Clone();
        public NativeArray<byte> GetDataArray_ForJob(Allocator allocator)
        {
            return new NativeArray<byte>(data, allocator);
        }

        private int bitsPerValue;
        private uint valueMask;

        public BitpackedScalarField(int width, int height, int depth)
        {
            if (width <= 0 || height <= 0 || depth <= 0)
                throw new ArgumentOutOfRangeException("Dimensions must be positive.");

            this.width = width;
            this.height = height;
            this.depth = depth;

            Initialize();
        }

        public int GetValue(int x, int y, int z)
        {
            ValidateCoordinates(x, y, z);
            long bitIndex = (long)CalculateIndex(x, y, z) * bitsPerValue;
            int byteIndex = (int)(bitIndex >> 3);
            int bitOffset = (int)(bitIndex & 7);

            ulong buffer = 0;
            int bytesToRead = (bitsPerValue + bitOffset + 7) >> 3;
            for (int i = 0; i < bytesToRead; i++)
            {
                if (byteIndex + i < data.Length)
                {
                    buffer |= (ulong)data[byteIndex + i] << (i * 8);
                }
            }

            return (int)((buffer >> bitOffset) & valueMask);
        }

        public void SetValue(int x, int y, int z, int value)
        {
            ValidateCoordinates(x, y, z);
            long bitIndex = (long)CalculateIndex(x, y, z) * bitsPerValue;
            int byteIndex = (int)(bitIndex >> 3);
            int bitOffset = (int)(bitIndex & 7);

            ulong writeValue = (uint)value & valueMask;
            ulong writeMask = (ulong)valueMask << bitOffset;

            writeValue <<= bitOffset;

            int bytesToWrite = (bitsPerValue + bitOffset + 7) >> 3;
            for (int i = 0; i < bytesToWrite; i++)
            {
                if (byteIndex + i < data.Length)
                {
                    data[byteIndex + i] = (byte)((data[byteIndex + i] & ~(writeMask >> (i * 8))) | (writeValue >> (i * 8)));
                }
            }
        }

        public void SetData(NativeArray<byte> newData)
        {
            newData.CopyTo(this.data);
        }

        public int CalculateIndex(int x, int y, int z)
            => z + y * depth + x * depth * height;

        public int this[int x, int y, int z]
        {
            get => GetValue(x, y, z);
            set => SetValue(x, y, z, value);
        }

        public bool IsInBounds(int x, int y, int z) =>
            x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth;

        public void Clear() => Array.Clear(data, 0, data.Length);

        private void ValidateCoordinates(int x, int y, int z)
        {
            if (!IsInBounds(x, y, z))
                throw new IndexOutOfRangeException($"Coordinates ({x}, {y}, {z}) are out of bounds.");
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            Initialize();
        }

        private void Initialize()
        {
            bitsPerValue = 1;

            valueMask = (1u << bitsPerValue) - 1;

            long totalBits = (long)width * height * depth * bitsPerValue;
            int byteSize = (int)((totalBits + 7) >> 3);

            if (data == null || data.Length != byteSize)
            {
                data = new byte[byteSize];
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        yield return this[x, y, z];
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}