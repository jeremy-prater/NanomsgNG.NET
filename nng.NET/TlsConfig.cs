using nng.Native;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace nng
{
    using static nng.Native.Defines;
    using static nng.Native.Tls.UnsafeNativeMethods;

    /// <summary>
    /// TLS configuration for dialers and listeners
    /// </summary>
    public class TlsConfig: INngTlsConfig
    {
        public nng_tls_config NativeNngStruct { get; protected set; }

        public static NngResult<INngTlsConfig> Alloc(nng_tls_mode mode)
        {
            int res = nng_tls_config_alloc(out var tlsConfig, mode);
            if (res != NNG_OK)
            {
                return NngResult<INngTlsConfig>.Fail(res);
            }
            return NngResult<INngTlsConfig>.Ok(new TlsConfig() { NativeNngStruct = tlsConfig});
        }

        public int SetServerName(string name)
            => nng_tls_config_server_name(NativeNngStruct, name);

        public int SetCaChain(string chain, string clr)
            => nng_tls_config_ca_chain(NativeNngStruct, chain, clr);

        public int SetCaChain(string chain)
            => nng_tls_config_ca_chain(NativeNngStruct, chain);

        public int SetOwnCert(string cert, string key, string passwd)
            => nng_tls_config_own_cert(NativeNngStruct, cert, key, passwd);

        public int SetOwnCert(string cert, string key)
            => nng_tls_config_own_cert(NativeNngStruct, cert, key);

        public int SetAuthMode(nng_tls_auth_mode mode)
            => nng_tls_config_auth_mode(NativeNngStruct, mode);

        public int SetCaFile(string path)
            => nng_tls_config_ca_file(NativeNngStruct, path);

        public int SetKeyFile(string path, string passwd)
            => nng_tls_config_cert_key_file(NativeNngStruct, path, passwd);

        public int SetKeyFile(string path)
            => nng_tls_config_cert_key_file(NativeNngStruct, path);

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
                // No managed resources to dispose
            }

            int _ = nng_tls_config_free(NativeNngStruct);

            disposed = true;
        }

        ~TlsConfig() => Dispose(false);

        bool disposed = false;
        #endregion

        private TlsConfig() { }
    }
}
