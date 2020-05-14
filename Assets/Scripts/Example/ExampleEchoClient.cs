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
			UDP,
			TCP
		}

		[SerializeField]
		private string serverIp = "127.0.0.1";

		[SerializeField]
		private int serverPort = 3333;

		[SerializeField]
		private int repeatMessage = 0;

		[SerializeField]
		private Type type = Type.TCP;

		[SerializeField]
		private bool reconnect = true;

		[SerializeField]
		private float reconnectionDelay = 1.0f;

		[SerializeField]
		private InputField inputField = null;

		[SerializeField]
		private Button button = null;

		[SerializeField]
		private Text serverResponseText = null;

		[SerializeField]
		private Text stateInfoText = null;

		private IUnitySocketClient socketClient;
		private byte[] buffer;

		private void Start()
		{
			switch (type)
			{
				case Type.UDP:
					socketClient = new UnityUdpClient(serverIp, serverPort);
					break;
				case Type.TCP:
					socketClient = new UnityTcpClient(serverIp, serverPort);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			buffer = new byte[socketClient.OptionReceiveBufferSize];

			socketClient.OnConnectedEvent += OnConnected;
			socketClient.OnDisconnectedEvent += OnDisconnected;
			socketClient.OnErrorEvent += OnError;
			socketClient.ConnectAsync();
			button.onClick.AddListener(OnSendEcho);
		}

		private void OnDestroy()
		{
			socketClient.OnConnectedEvent -= OnConnected;
			socketClient.OnDisconnectedEvent -= OnDisconnected;
			socketClient.OnErrorEvent -= OnError;

			reconnect = false;
			socketClient.Disconnect();
		}

		private void Update()
		{
			UpdateStateInfoText();
			if (!socketClient.IsConnected)
			{
				button.interactable = false;
				return;
			}

			button.interactable = true;

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

			serverResponseText.text = messages;
		}

		private void UpdateStateInfoText()
		{
			var text = $"Server ip: {socketClient.Endpoint.Address}, Server port: {socketClient.Endpoint.Port}\n" +
						$"Connecting: {socketClient.IsConnecting}, IsConnected: {socketClient.IsConnected}\n ";
			stateInfoText.text = text;
		}

		private void OnSendEcho()
		{
			var message = Encoding.UTF8.GetBytes(inputField.text);
			socketClient.Send(message);

			for (int i = 0; i < repeatMessage; i++)
			{
				socketClient.Send(message);
			}
		}

		private void OnConnected()
		{
			Debug.Log($"TCP client connected a new session with Id {socketClient.Id}");
		}

		private void OnDisconnected()
		{
			Debug.Log($"TCP client disconnected a session with Id {socketClient.Id}");

			if (reconnect)
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
			Debug.LogError($"TCP client caught an error with code {error}");
		}
	}
}
