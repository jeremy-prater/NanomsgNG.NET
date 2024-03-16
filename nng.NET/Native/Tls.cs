using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace nng.Native.Tls
{
    using static Globals;

#if NETSTANDARD2_0
    [System.Security.SuppressUnmanagedCodeSecurity]
#endif
    public sealed class UnsafeNativeMethods
    {
        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_tls_config_alloc(out nng_tls_config config, nng_tls_mode mode);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_tls_config_free(nng_tls_config config);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_tls_config_server_name(nng_tls_config config, string name);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        static extern Int32 nng_tls_config_ca_chain(nng_tls_config config, string chain, IntPtr crl);

        public static Int32 nng_tls_config_ca_chain(nng_tls_config config, string chain, string clr)
        {
            IntPtr clrPtr = Marshal.StringToHGlobalAuto(clr);
            return nng_tls_config_ca_chain(config, chain, clrPtr);
        }

        public static Int32 nng_tls_config_ca_chain(nng_tls_config config, string chain)
        {
            return nng_tls_config_ca_chain(config, chain, IntPtr.Zero);
        }

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        static extern Int32 nng_tls_config_own_cert(nng_tls_config config, string cert, string key, IntPtr passwd);

        public static Int32 nng_tls_config_own_cert(nng_tls_config config, string cert, string key, string passwd)
        {
            IntPtr passwdPtr = Marshal.StringToHGlobalAuto(passwd);
            return nng_tls_config_own_cert(config, cert, key, passwdPtr);
        }

        public static Int32 nng_tls_config_own_cert(nng_tls_config config, string cert, string key)
        {
            return nng_tls_config_own_cert(config, cert, key, IntPtr.Zero);
        }

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_tls_config_auth_mode(nng_tls_config config, nng_tls_auth_mode mode);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_tls_config_ca_file(nng_tls_config config, string path);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        static extern Int32 nng_tls_config_cert_key_file(nng_tls_config config, string path, IntPtr passwd);

        public static Int32 nng_tls_config_cert_key_file(nng_tls_config config, string path, string passwd)
        {
            IntPtr passwdPtr = Marshal.StringToHGlobalAuto(passwd);
            return nng_tls_config_cert_key_file(config, path, passwdPtr);
        }

        public static Int32 nng_tls_config_cert_key_file(nng_tls_config config, string path)
        {
            return nng_tls_config_cert_key_file(config, path, IntPtr.Zero);
        }
    }
}