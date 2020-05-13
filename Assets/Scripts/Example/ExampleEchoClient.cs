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
using System.Text;
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
		private InputField inputField = null;

		[SerializeField]
		private Button button = null;

		[SerializeField]
		private Text serverResponseText = null;

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

			socketClient.Connect();
			button.onClick.AddListener(OnSendEcho);
		}

		private void OnDestroy()
		{
			socketClient.DisconnectAndStop();
		}

		private void Update()
		{
			if (!socketClient.IsConnected)
			{
				button.interactable = false;
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
				Debug.Log(message);
				messages += message + "\n";
			}

			serverResponseText.text = messages;
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
	}
}