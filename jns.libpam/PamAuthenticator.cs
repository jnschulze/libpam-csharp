using System;
using System.Runtime.InteropServices;
using System.Text;
using jns.libpam.Interop;

namespace jns.libpam
{
    public unsafe class PamAuthenticator : IDisposable
    {
        private IntPtr _pamConv;
        private IntPtr _currentPassword;

        public PamAuthenticator()
        {
            _pamConv = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Libpam.pam_conv)));
            var convPtr = (Libpam.pam_conv*)_pamConv.ToPointer();

            convPtr->opaque = IntPtr.Zero;
            convPtr->cb = Marshal.GetFunctionPointerForDelegate(new Libpam.PasswordCallback(GetPasswordCallback));
        }

        public unsafe bool TryAuthenticate(string username, string password, string service = "su")
        {
            IntPtr ctx = IntPtr.Zero;

            var result = Libpam.pam_start(service, username, _pamConv, ref ctx);

            if (result != 0)
                return false;
                
            // will be freed by libpam
            _currentPassword = StringToCString(password);

            result = Libpam.pam_authenticate(ctx, Libpam.PAM_SILENT);

            Libpam.pam_end(ctx, result);

            return result == 0;
        }

        private unsafe int GetPasswordCallback(int numMsg, Libpam.pam_message** pamMessage, ref IntPtr pamResponse, IntPtr opaque)
        {
            // We mustn't use Marshall.AllocHGlobal here
            // as the response will be freed by libpam using libc's free()
            pamResponse = Libc.Malloc(Marshal.SizeOf(typeof(Libpam.pam_response)));

            var ptr = (Libpam.pam_response*)pamResponse.ToPointer();

            ptr->ret = 0;
            ptr->response = _currentPassword;

            return 0;
        }

        private static IntPtr StringToCString(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str + Char.MinValue);

            var buf = Libc.Malloc(bytes.Length);

            Marshal.Copy(bytes, 0, buf, bytes.Length);

            return buf;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            if (_pamConv != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_pamConv);
                _pamConv = IntPtr.Zero;
            }
        }
    }
}