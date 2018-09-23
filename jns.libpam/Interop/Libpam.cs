using System;
using System.Runtime.InteropServices;

namespace jns.libpam.Interop
{
    internal static class Libpam
    {
        const string Lib = "libpam";

        public const uint PAM_SILENT = 0x80000000;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct pam_message
        {
            public int msg_style;
            public char* msg;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct pam_response
        {
            public IntPtr response;
            public int ret;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct pam_conv
        {
            public IntPtr cb;
            public IntPtr opaque;
        }

        [DllImport(Lib)]
        public static unsafe extern int pam_start(
            [MarshalAs(UnmanagedType.LPStr)] string service_name,
            [MarshalAs(UnmanagedType.LPStr)] string user,
            IntPtr conv,
            ref IntPtr pamHandle
        );

        [DllImport(Lib)]
        public static extern int pam_authenticate(IntPtr pamHandle, uint flags);

        [DllImport(Lib)]
        public static extern int pam_end(IntPtr pamHandle, int lastStatus);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int PasswordCallback(int numMsg, pam_message** pamMessage, ref IntPtr pamResponse, IntPtr opaque);
    }
}
