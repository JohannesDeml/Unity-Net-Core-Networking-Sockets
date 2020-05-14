// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitySocketClient.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;

namespace NetCoreServer
{
	public interface IUnitySocketClient
	{
		/// <summary>
		/// Invoked when the client connects to the server from the connection thread.
		/// Do not call any unity objects from this callback since they are only allowed from the main thread.
		/// </summary>
		event Action OnConnectedEvent;

		/// <summary>
		/// Invoked when the client disconnects from the server from the connection thread.
		/// Do not call any unity objects from this callback since they are only allowed from the main thread.
		/// </summary>
		event Action OnDisconnectedEvent;

		/// <summary>
		/// Invoked when the socket has an error from the socket thread.
		/// Do not call any unity objects from this callback since they are only allowed from the main thread.
		/// </summary>
		event Action<SocketError> OnErrorEvent;

		Guid Id { get; }
		bool IsConnecting { get; }
		bool IsConnected { get; }
		IPEndPoint Endpoint { get; }
		Socket Socket { get; }
		bool Connect();
		bool ConnectAsync();
		bool Disconnect();
		bool Reconnect();
		bool ReconnectAsync();


		int OptionReceiveBufferSize { get; set; }
		int OptionSendBufferSize { get; set; }
		long Send(byte[] buffer);
		long Send(byte[] buffer, long offset, long size);
		long Send(string text);
		bool SendAsync(byte[] buffer);
		bool SendAsync(byte[] buffer, long offset, long size);
		bool SendAsync(string text);

		/// <summary>
		/// Check if there are any new received messages
		/// </summary>
		/// <returns>True, if there are any new received messages in the queue</returns>
		bool HasEnqueuedPackages();

		/// <summary>
		/// Dequeue the oldest received message with a provided array in order to generate no garbage
		/// </summary>
		/// <param name="array">array to store the message to</param>
		/// <returns>byte length stored to the array</returns>
		int GetNextPackage(ref byte[] array);
	}
}
