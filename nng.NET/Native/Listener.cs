using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace nng.Native.Listener
{
    using static Globals;

#if NETSTANDARD2_0
    [System.Security.SuppressUnmanagedCodeSecurity]
#endif
    public sealed class UnsafeNativeMethods
    {
        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_create(out nng_listener listener, nng_socket socket, string url);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_start(nng_listener listener, Defines.NngFlag flags);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_close(nng_listener listener);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_id(nng_listener listener);


        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get(nng_listener listener, string name, byte[] data, UIntPtr size);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_bool(nng_listener listener, string name, out bool data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_int(nng_listener listener, string name, out Int32 data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_ms(nng_listener listener, string name, out nng_duration data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_size(nng_listener listener, string name, out UIntPtr data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_addr(nng_listener listener, string name, out nng_sockaddr data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_string(nng_listener listener, string name, out IntPtr data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_uint64(nng_listener listener, string name, out UInt64 data);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_get_ptr(nng_listener listener, string name, out IntPtr data);


        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set(nng_listener listener, string name, byte[] data, UIntPtr size);

        public static int nng_listener_set(nng_listener listener, string name, byte[] data)
        {
            return nng_listener_set(listener, name, data, (UIntPtr)data.Length);
        }

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_bool(nng_listener listener, string name, bool value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_int(nng_listener listener, string name, Int32 value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_ms(nng_listener listener, string name, nng_duration value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_size(nng_listener listener, string name, UIntPtr value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_uint64(nng_listener listener, string name, UInt64 value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_ptr(nng_listener listener, string name, IntPtr value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int nng_listener_set_string(nng_listener listener, string name, string value);
    }
}