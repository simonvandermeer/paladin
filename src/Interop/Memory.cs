using System;
using System.Runtime.InteropServices;

namespace Paladin.Interop
{
    /// <summary>
    /// Based on https://stackoverflow.com/questions/50672268/c-sharp-reading-another-process-memory
    /// </summary>
    internal static class Memory
    {
        public static T Read<T>(IntPtr processHandle, IntPtr addr, out int bytesRead) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));

            var buffer = Read(processHandle, addr, size, out bytesRead);

            return ByteArrayToStructure<T>(buffer);
        }

        public static byte[] Read(IntPtr processHandle, IntPtr addr, int size, out int bytesRead)
        {
            var buffer = new byte[size];

            Imports.NtReadVirtualMemory(processHandle, addr, buffer, buffer.Length, out bytesRead);

            return buffer;
        }

        public static IntPtr ReadPointer(IntPtr processHandle, IntPtr addr)
        {
            var buffer = Read(processHandle, addr, 32, out _);

            return new IntPtr(BitConverter.ToInt32(buffer));
        }

        #region Conversion

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        #endregion
    }
}
