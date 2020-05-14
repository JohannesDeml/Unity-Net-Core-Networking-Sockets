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
		event Action OnConnectedEvent;
		event Action OnDisconnectedEvent;
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

		bool HasEnqueuedPackages();
		int GetNextPackage(ref byte[] array);
	}
}
