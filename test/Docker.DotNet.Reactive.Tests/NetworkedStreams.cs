using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Docker.DotNet.Reactive.Tests
{
	/// <summary>
    ///		Quick-and-dirty hack to simulate Docker's streaming API. 
    /// </summary>
	/// <remarks>
    ///		Creates 2 streaming sockets and connects them to each other. 
    /// </remarks>
    public sealed class NetworkedStreams
        : IDisposable
    {
		readonly Socket _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		readonly Socket _writeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		/// <summary>
        ///		Create a new <see cref="NetworkedStreams"/>. 
        /// </summary>
        public NetworkedStreams()
		{
		}

		/// <summary>
        ///		Dispose of resources being used by the <see cref="NetworkedStreams"/>.
        /// </summary>
		void IDisposable.Dispose()
		{
			Reader?.Dispose();
			Read?.Dispose();

			Writer?.Dispose();
			Write?.Dispose();

			_listenSocket.Dispose();
			_writeSocket.Dispose();
		}

		/// <summary>
        /// 	A <see cref="StreamReader"/> that reads data from the input stream.
        /// </summary>
		public StreamReader Reader { get; private set; }

		/// <summary>
        /// 	A <see cref="StreamReader"/> that writes data to the output stream.
        /// </summary>
		public StreamWriter Writer { get; private set; }

		/// <summary>
        /// 	The incoming network stream.
        /// </summary>
		NetworkStream Read { get; set; }

		/// <summary>
        /// 	The outgoing network stream.
        /// </summary>
		NetworkStream Write { get; set; } 

		/// <summary>
        ///		Open the streams. 
        /// </summary>
		public void Open()
		{
			if (Read != null || Write != null)
				throw new InvalidOperationException("NetworkedStreams has already been started.");

			_listenSocket.Bind(new IPEndPoint(
				IPAddress.Loopback, 0
			));
			_listenSocket.Listen(1);

			// Asynchronously accept...
			Task<Socket> acceptTask = _listenSocket.AcceptAsync();

			// ...and connect.
			_writeSocket.Connect(_listenSocket.LocalEndPoint);
			
			Write = new NetworkStream(_writeSocket);
			Writer = new StreamWriter(Write);
			Writer.AutoFlush = true;
			
			Read = new NetworkStream(acceptTask.Result);
			Reader = new StreamReader(Read);
		}

		/// <summary>
        ///		Close the streams. 
        /// </summary>
		public void Close()
		{
			((IDisposable)this).Dispose();
		}
    }
}