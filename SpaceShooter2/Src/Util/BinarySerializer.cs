#nullable enable
using System;
using System.Runtime.InteropServices;

namespace SpaceShooter2.Src.Util;

public static class BinarySerializer
{
    public static unsafe T? Deserialize<T>(byte[] buf) where T : struct
    {
        // I am assuming that the programmer is not stupid and will read from the correct type of buffer, hence me not checking the sizes
        T? obj;

        // get the array as a pointer
        fixed (byte* pBuf = buf)
        {
            // store the data of the pointer as the desired object
            obj = Marshal.PtrToStructure<T>((IntPtr)pBuf);
        }

        return obj;
    }

    public static unsafe byte[] Serialize<T>(T obj) where T : struct
    {
        byte[] buf = new byte[Marshal.SizeOf<T>()]; // allocate an array the size of T

        // get the array as a pointer (all arrays are a pointer)
        fixed (byte* pBuf = buf)
        {
            // store the structure to the pointer which points to the buffer
            Marshal.StructureToPtr(obj, (IntPtr)pBuf, false);
        }

        return buf;
    }
}
