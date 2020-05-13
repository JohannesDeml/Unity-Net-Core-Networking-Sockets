// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitySocketClient.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace NetCoreServer
{
	public interface IUnitySocketClient
	{
		bool IsConnected { get; }
		bool Connect();
		bool Disconnect();
		bool Reconnect();
		void DisconnectAndStop();
		

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