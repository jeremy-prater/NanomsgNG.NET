using nng.Native;
using System;
using System.Threading;
using System.Threading.Tasks;

using static nng.Native.Defines;

namespace nng
{
    public class NngException : Exception
    {
        public NngException(int errorCode, string message = "")
            : base(MessageFormat(errorCode, message))
        {
            ErrorCode = errorCode;
        }

        public NngException(NngErrno errno, string message = "")
            : this((int)errno, message)
        {
        }

        public static void AssertZero(int errorCode, string message = "")
        {
            if (errorCode != NNG_OK)
                throw new NngException(errorCode, message);
        }

        private static string MessageFormat(int errorCode, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                return ((NngErrno)errorCode).ToString();
            }
            else
            {
                return ((NngErrno)errorCode).ToString() + ": " + message;
            }
        }

        public int ErrorCode { get; } = 0;
        public NngErrno Error => (NngErrno)ErrorCode;
    }

    /// <summary>
    /// Operations on part of a message; either the header or body
    /// </summary>
    public interface INngMsgPart
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        int Append(IntPtr data, int size);
        int Append(ReadOnlySpan<byte> data);
        int Append(uint data);
        int Chop(UIntPtr size);
        int Chop(out uint data);
        void Clear();
        int Insert(byte[] data);
        int Insert(uint data);
        int Length { get; }
        int Trim(UIntPtr size);
        int Trim(out uint data);
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        Span<byte> AsSpan();
        /// <summary>
        /// Get pointer to unmanaged memory.
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        IntPtr AsPtr();
    }

    /// <summary>
    /// Message sent/received via nng
    /// </summary>
    public interface INngMsg : INngMsgPart, IDisposable
    {
        /// <summary>
        /// Get the underlying nng_msg
        /// </summary>
        /// <value></value>
        nng_msg NativeNngStruct { get; }
        /// <summary>
        /// Take ownership of he underlying nng_msg
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        nng_msg Take();
        /// <summary>
        /// Get the header portion of the message
        /// </summary>
        /// <value></value>
        INngMsgPart Header { get; }
        /// <summary>
        /// Duplicates the message creating a new, identical message.
        /// </summary>
        /// <returns>The newly created identical message duplicate</returns>
        NngResult<INngMsg> Dup();
        /// <summary>
        /// Get the pipe object associated with the message.
        /// </summary>
        INngPipe Pipe { get; }
    }

    /// <summary>
    /// Handle to a "pipe", which can be thought of as a single connection.
    /// </summary>
    public interface INngPipe : IGetOptions
    {
        /// <summary>
        /// Get the underlying nng_pipe.
        /// </summary>
        nng_pipe NativeNngStruct { get; }

        /// <summary>
        /// A positive identifier for the pipe, if it is valid.
        /// </summary>
        int Id { get; }

        [Obsolete("TODO: return instance of correct wrapped socket type")]
        nng_socket Socket { get; }
        INngDialer Dialer { get; }
        INngListener Listener { get; }

        NngResult<Unit> Close();
    }

    public static partial class Extensions
    {
        public static TaskCompletionSource<NngResult<T>> CreateSource<T>()
        {
            return new TaskCompletionSource<NngResult<T>>(TaskCreationOptions.RunContinuationsAsynchronously);
        }
        public static CancellationTokenTaskSource<NngResult<T>> CreateReceiveSource<T>(CancellationToken token)
        {
            return new CancellationTokenTaskSource<NngResult<T>>(token, TaskCreationOptions.RunContinuationsAsynchronously);
        }
        public static TaskCompletionSource<NngResult<Unit>> CreateSendResultSource()
        {
            return new TaskCompletionSource<NngResult<Unit>>(TaskCreationOptions.RunContinuationsAsynchronously);
        }
        public static void SetNngResult(this TaskCompletionSource<NngResult<Unit>> socket)
        {
            socket.SetResult(Unit.Ok);
        }
        public static void SetNngResult<T>(this TaskCompletionSource<NngResult<T>> socket, T message)
        {
            socket.SetResult(NngResult<T>.Ok(message));
        }
        public static void TrySetNngResult<T>(this TaskCompletionSource<NngResult<T>> socket, T message)
        {
            socket.TrySetResult(NngResult<T>.Ok(message));
        }
        public static void TrySetNngResult(this TaskCompletionSource<NngResult<Unit>> socket)
        {
            socket.TrySetResult(Unit.Ok);
        }
        public static void TrySetNngResult<T>(this CancellationTokenTaskSource<NngResult<T>> socket, T message)
        {
            socket.TrySetResult(NngResult<T>.Ok(message));
        }
        public static void TrySetNngError<T>(this TaskCompletionSource<NngResult<T>> socket, NngErrno error)
        {
            socket.TrySetResult(NngResult<T>.Fail(error));
        }
        public static void TrySetNngError<T>(this CancellationTokenTaskSource<NngResult<T>> socket, NngErrno error)
        {
            socket.TrySetResult(NngResult<T>.Fail(error));
        }

        public static bool IsValid(this INngSocket self)
        {
            return self.Id != -1;
        }
        public static bool IsValid(this INngListener self)
        {
            return self.Id != -1;
        }
        public static bool IsValid(this INngDialer self)
        {
            return self.Id != -1;
        }
        public static bool IsValid(this INngPipe self)
        {
            return self.Id != -1;
        }
    }
}