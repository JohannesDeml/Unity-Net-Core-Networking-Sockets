// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityTcpClient.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace NetCoreServer
{
	class UnityTcpClient : TcpClient, IUnitySocketClient
	{
		public event Action OnConnectedEvent;
		public event Action OnDisconnectedEvent;
		public event Action<SocketError> OnErrorEvent;

		private MemoryStream queueBuffer;
		private Queue<BufferPointer> queueBufferPointer;


		/// <inheritdoc />
		public UnityTcpClient(string address, int port) : base(address, port)
		{
			queueBuffer = new MemoryStream(OptionReceiveBufferSize);
			queueBufferPointer = new Queue<BufferPointer>();
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
			OnConnectedEvent?.Invoke();
			// Start receive datagrams
			ReceiveAsync();
		}

		protected override void OnDisconnected()
		{
			OnDisconnectedEvent?.Invoke();
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
			OnErrorEvent?.Invoke(error);
		}
	}
}
