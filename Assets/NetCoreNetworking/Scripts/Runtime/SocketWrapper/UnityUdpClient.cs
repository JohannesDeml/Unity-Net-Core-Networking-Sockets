// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityUdpClient.cs">
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
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace NetCoreServer
{
	public class UnityUdpClient : UdpClient, IUnitySocketClient
	{
		/// <inheritdoc />
		public event Action OnConnectedEvent;
		/// <inheritdoc />
		public event Action OnDisconnectedEvent;
		/// <inheritdoc />
		public event Action<SocketError> OnErrorEvent;

		private MemoryStream queueBuffer;
		private Queue<BufferPointer> queueBufferPointer;


		/// <inheritdoc />
		public UnityUdpClient(string address, int port) : base(address, port)
		{
			queueBuffer = new MemoryStream(OptionReceiveBufferSize);
			queueBufferPointer = new Queue<BufferPointer>();
		}

		/// <inheritdoc />
		public bool IsConnecting { get; private set; } = false;

		/// <summary>
		/// Same as Connect, since async is not necessary for udp sockets
		/// </summary>
		/// <returns>True if connection is established</returns>
		public bool ConnectAsync()
		{
			// No need to do this async fro udp
			return Connect();
		}

		/// <inheritdoc />
		public bool ReconnectAsync()
		{
			// No need to do this async fro udp
			return Reconnect();
		}

		/// <inheritdoc />
		public bool HasEnqueuedPackages()
		{
			return queueBufferPointer.Count > 0;
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
		protected override void OnConnected()
		{
			try
			{
				OnConnectedEvent?.Invoke();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			// Start receive datagrams
			ReceiveAsync();
		}

		/// <inheritdoc />
		protected override void OnDisconnected()
		{
			try
			{
				OnDisconnectedEvent?.Invoke();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		/// <inheritdoc />
		protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
		{
			var start = (int) queueBuffer.Length;
			queueBuffer.Write(buffer, (int) offset, (int) size);
			queueBufferPointer.Enqueue(new BufferPointer(start, (int) size));

			// Continue receive datagrams
			ReceiveAsync();
		}

		/// <inheritdoc />
		protected override void OnError(SocketError error)
		{
			try
			{
				OnErrorEvent?.Invoke(error);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}
	}
}
