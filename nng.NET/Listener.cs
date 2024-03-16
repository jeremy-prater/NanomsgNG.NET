using nng.Native;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace nng
{
    using static nng.Native.Defines;
    using static nng.Native.Listener.UnsafeNativeMethods;

    /// <summary>
    /// Used to accept a connection from a <see cref="IDialer"/>
    /// </summary>
    public class NngListener : INngListener
    {
        public nng_listener NativeNngStruct { get; protected set; }
        public int Id => nng_listener_id(NativeNngStruct);

        public static INngListener Create(INngSocket socket, string url)
        {
            int res = nng_listener_create(out var listener, socket.NativeNngStruct, url);
            if (res != NNG_OK)
            {
                return null;
            }
            return new NngListener { NativeNngStruct = listener };
        }

        public static INngListener Create(nng_listener listener)
        {
            return new NngListener { NativeNngStruct = listener };
        }

        public int Start(NngFlag flags)
            => nng_listener_start(NativeNngStruct, flags);

        public int GetOpt(string name, out bool data)
            => nng_listener_get_bool(NativeNngStruct, name, out data);
        public int GetOpt(string name, out int data)
            => nng_listener_get_int(NativeNngStruct, name, out data);
        public int GetOpt(string name, out nng_duration data)
            => nng_listener_get_ms(NativeNngStruct, name, out data);
        public int GetOpt(string name, out IntPtr data)
            => nng_listener_get_ptr(NativeNngStruct, name, out data);
        public int GetOpt(string name, out UIntPtr data)
            => nng_listener_get_size(NativeNngStruct, name, out data);
        public int GetOpt(string name, out nng_sockaddr data)
            => nng_listener_get_addr(NativeNngStruct, name, out data);
        public int GetOpt(string name, out string data)
        {
            IntPtr ptr;
            var res = nng_listener_get_string(NativeNngStruct, name, out ptr);
            data = NngString.Create(ptr).ToManaged();
            return res;
        }
        public int GetOpt(string name, out UInt64 data)
            => nng_listener_get_uint64(NativeNngStruct, name, out data);

        public int SetOpt(string name, byte[] data)
            => nng_listener_set(NativeNngStruct, name, data);
        public int SetOpt(string name, bool data)
            => nng_listener_set_bool(NativeNngStruct, name, data);
        public int SetOpt(string name, int data)
            => nng_listener_set_int(NativeNngStruct, name, data);
        public int SetOpt(string name, nng_duration data)
            => nng_listener_set_ms(NativeNngStruct, name, data);
        public int SetOpt(string name, UIntPtr data)
            => nng_listener_set_size(NativeNngStruct, name, data);
        public int SetOpt(string name, UInt64 data)
            => nng_listener_set_uint64(NativeNngStruct, name, data);
        public int SetOpt(string name, IntPtr data)
            => nng_listener_set_ptr(NativeNngStruct, name, data);
        public int SetOpt(string name, string data)
            => nng_listener_set_string(NativeNngStruct, name, data);

        private NngTlsConfig tlsConfig = null;
        public NngTlsConfig TlsConfig { 
            get => tlsConfig;
            set
            {
                if (value != null)
                {
                    int res = SetOpt(NNG_OPT_TLS_CONFIG, value.NativeNngStruct.ptr);
                    if (res == NNG_OK)
                    {
                        tlsConfig = value;
                    }
                }
            }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                tlsConfig?.Dispose();
            }

            int _ = nng_listener_close(NativeNngStruct);

            disposed = true;
        }

        ~NngListener() => Dispose(false);

        bool disposed = false;
        #endregion

        private NngListener() { }
    }
}
