using nng.Native;
using System;

namespace nng
{
    public interface INngTlsConfig : IDisposable
    {
        nng_tls_config NativeNngStruct { get; }
        int SetServerName(string name);
        int SetCaChain(string chain, string clr);
        int SetCaChain(string chain);
        int SetOwnCert(string cert, string key, string passwd);
        int SetOwnCert(string cert, string key);
        int SetAuthMode(nng_tls_auth_mode mode);
        int SetCaFile(string path);
        int SetKeyFile(string path, string passwd);
        int SetKeyFile(string path);
    }
}