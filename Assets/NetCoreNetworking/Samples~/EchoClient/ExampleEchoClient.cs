// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleEchoClient.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetCoreServer;
using UnityEngine;
using UnityEngine.UI;

namespace Supyrb
{
	public class ExampleEchoClient : MonoBehaviour
	{
		public enum Type
		{
			Ssl,
			Tcp,
			Udp
		}

		#region SettingFields

		[SerializeField]
		private string serverIp = "127.0.0.1";

		[SerializeField]
		private int serverPort = 3333;

		[Tooltip("Number of times the message is repeated to simulate more requests.")]
		[SerializeField]
		private int repeatMessage = 0;

		[Tooltip("Non-blocking async sending. Not recommended for UDP (those are already non-blocking)")]
		[SerializeField]
		private bool sendAsync = true;

		[SerializeField]
		private Type type = Type.Tcp;

		[SerializeField, Tooltip("Only needed for SSL Sockets")]
		private SslCertificateAsset sslCertificateAsset = null;

		[Tooltip("Try to reconnect if connection could not be established or was lost")]
		[SerializeField]
		private bool reconnect = true;

		[SerializeField]
		private float reconnectionDelay = 1.0f;

		#endregion
		
		#region UiFields

		[Header("Settings Input")]
		[SerializeField]
		private InputField serverIpInput = null;

		[SerializeField]
		private InputField serverPortInput = null;

		[SerializeField]
		private Button sslConnectButton = null;

		[SerializeField]
		private Button tcpConnectButton = null;

		[SerializeField]
		private Button udpConnectButton = null;

		[SerializeField]
		private Button disconnectButton = null;

		[Header("Connection")]
		[SerializeField]
		private InputField messageInputField = null;

		[SerializeField]
		private Button sendMessageButton = null;

		[SerializeField]
		private Text serverResponseText = null;

		[SerializeField]
		private Text stateInfoText = null;

		#endregion

		// Used implementation as interface to allow easy switching
		private IUnitySocketClient socketClient;

		// Buffer will be used for sending and receiving to avoid creating garbage
		private byte[] buffer;
		private bool disconnecting;
		private bool applicationQuitting;

		private void Start()
		{
			disconnecting = false;
			tcpConnectButton.onClick.AddListener(TriggerTcpConnect);
			sslConnectButton.onClick.AddListener(TriggerSslConnect);
			udpConnectButton.onClick.AddListener(TriggerUdpConnect);
			disconnectButton.onClick.AddListener(TriggerDisconnect);
			sendMessageButton.onClick.AddListener(OnSendEcho);
		}

		private void OnDestroy()
		{
			Disconnect();
		}

		[ContextMenu("Connect")]
		private void Connect()
		{
			if (socketClient != null && (socketClient.IsConnected || socketClient.IsConnecting))
			{
				Disconnect();
				socketClient = null;
			}

			switch (type)
			{
				case Type.Ssl:
					var sslContext = sslCertificateAsset.GetContext();
					socketClient = new UnitySslClient(sslContext, serverIp, serverPort);
					break;
				case Type.Tcp:
					socketClient = new UnityTcpClient(serverIp, serverPort);
					break;
				case Type.Udp:
					socketClient = new UnityUdpClient(serverIp, serverPort);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			buffer = new byte[socketClient.OptionReceiveBufferSize];

			socketClient.OnConnectedEvent += OnConnected;
			socketClient.OnDisconnectedEvent += OnDisconnected;
			socketClient.OnErrorEvent += OnError;
			socketClient.ConnectAsync();
		}

		[ContextMenu("Disconnect")]
		private void Disconnect()
		{
			if (socketClient == null)
			{
				return;
			}

			disconnecting = true;
			socketClient.Disconnect();

			while (socketClient.IsConnected)
			{
				Thread.Yield();
			}

			socketClient.OnConnectedEvent -= OnConnected;
			socketClient.OnDisconnectedEvent -= OnDisconnected;
			socketClient.OnErrorEvent -= OnError;
			disconnecting = false;
		}

		private void Update()
		{
			UpdateStateInfoText();
			bool connected = socketClient != null && socketClient.IsConnected;
			sendMessageButton.interactable = connected;
			disconnectButton.interactable = connected;
			sslConnectButton.interactable = !connected;
			tcpConnectButton.interactable = !connected;
			udpConnectButton.interactable = !connected;

			if (!connected)
			{
				return;
			}

			if (!socketClient.HasEnqueuedPackages())
			{
				return;
			}

			string messages = $"Messages received at frame {Time.frameCount}:\n";
			while (socketClient.HasEnqueuedPackages())
			{
				int length = socketClient.GetNextPackage(ref buffer);
				var message = Encoding.UTF8.GetString(buffer, 0, length);
				messages += message + "\n";
			}

			serverResponseText.text = messages + serverResponseText.text;
		}

		private void UpdateStateInfoText()
		{
			if (socketClient == null)
			{
				return;
			}

			var text = $"Server ip: {socketClient.Endpoint.Address}, Server port: {socketClient.Endpoint.Port}\n" +
						$"Connecting: {socketClient.IsConnecting}, IsConnected: {socketClient.IsConnected}\n ";
			stateInfoText.text = text;
		}

		private void OnConnected()
		{
			Debug.Log($"{socketClient.GetType()} connected a new session with Id {socketClient.Id}");
		}

		private void OnDisconnected()
		{
			Debug.Log($"{socketClient.GetType()} disconnected a session with Id {socketClient.Id}");
			if (applicationQuitting)
			{
				return;
			}

			if (reconnect && !disconnecting)
			{
				ReconnectDelayedAsync();
			}
		}

		private async void ReconnectDelayedAsync()
		{
			await Task.Delay((int) (reconnectionDelay * 1000));

			if (socketClient.IsConnected || socketClient.IsConnecting)
			{
				return;
			}

			Debug.Log("Trying to reconnect");
			socketClient.ConnectAsync();
		}

		private void OnError(SocketError error)
		{
			Debug.LogError($"{socketClient.GetType()} caught an error with code {error}");
		}

		private void TriggerSslConnect()
		{
			type = Type.Ssl;
			ApplyInputAndConnect();
		}

		private void TriggerTcpConnect()
		{
			type = Type.Tcp;
			ApplyInputAndConnect();
		}

		private void TriggerUdpConnect()
		{
			type = Type.Udp;
			ApplyInputAndConnect();
		}

		private void TriggerDisconnect()
		{
			Disconnect();
		}

		private void ApplyInputAndConnect()
		{
			serverIp = serverIpInput.text;
			serverPort = int.Parse(serverPortInput.text);
			Connect();
		}

		[ContextMenu("Send message")]
		private void OnSendEcho()
		{
			var message = Encoding.UTF8.GetBytes(messageInputField.text);
			if (sendAsync)
			{
				SendAsync(message);
			}
			else
			{
				Send(message);
			}
		}

		private void Send(byte[] message)
		{
			for (int i = 0; i < 1 + repeatMessage; i++)
			{
				socketClient.Send(message);
			}
		}

		private void SendAsync(byte[] message)
		{
			for (int i = 0; i < 1 + repeatMessage; i++)
			{
				socketClient.SendAsync(message);
			}
		}

		private void OnApplicationQuit()
		{
			applicationQuitting = true;
		}
	}
}
