using System;
using System.Runtime.InteropServices;

namespace jns.libpam.Interop
{
    internal static class Libc
    {
        [DllImport("libc", EntryPoint = "malloc")]
        public static extern IntPtr Malloc(int size);

        [DllImport("libc", EntryPoint = "free")]
        public static extern void Free(IntPtr ptr);
    }
}