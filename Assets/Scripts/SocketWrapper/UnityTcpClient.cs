// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityTcpClient.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace NetCoreServer
{
	class UnityTcpClient : TcpClient, IUnitySocketClient
	{
		private bool stop;
		private MemoryStream queueBuffer;
		private Queue<BufferPointer> queueBufferPointer;


		/// <inheritdoc />
		public UnityTcpClient(string address, int port) : base(address, port)
		{
			queueBuffer = new MemoryStream(OptionReceiveBufferSize);
			queueBufferPointer = new Queue<BufferPointer>();
		}

		public void DisconnectAndStop()
		{
			stop = true;
			Disconnect();
			while (IsConnected)
			{
				Thread.Yield();
			}
		}

		public bool HasEnqueuedPackages()
		{
			return queueBufferPointer.Count > 0;
		}

		public int GetNextPackage(ref byte[] array)
		{
			if (queueBufferPointer.Count == 0)
			{
				return -1;
			}

			var pointer = queueBufferPointer.Dequeue();
			var lastPosition = queueBuffer.Position;
			queueBuffer.Position = pointer.Offset;
			queueBuffer.Read(array, 0, pointer.Length);

			if (queueBufferPointer.Count == 0)
			{
				// All packages read, clear memory stream
				queueBuffer.SetLength(0L);
			}
			else
			{
				queueBuffer.Position = lastPosition;
			}

			return pointer.Length;
		}

		protected override void OnConnected()
		{
			Debug.Log($"TCP client connected a new session with Id {Id}");
			// Start receive datagrams
			ReceiveAsync();
		}

		protected override void OnDisconnected()
		{
			Debug.Log($"TCP client disconnected a session with Id {Id}");
			// Wait for a while...
			Thread.Sleep(1000);

			// Try to connect again
			if (!stop)
			{
				Connect();
			}
		}

		protected override void OnReceived(byte[] buffer, long offset, long size)
		{
			var start = (int) queueBuffer.Length;
			queueBuffer.Write(buffer, (int) offset, (int) size);
			queueBufferPointer.Enqueue(new BufferPointer(start, (int) size));

			// Continue receive datagrams
			ReceiveAsync();
		}

		protected override void OnError(SocketError error)
		{
			Debug.LogError($"TCP client caught an error with code {error}");
		}
	}
}