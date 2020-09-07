// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleEchoClientUi.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;
using NetCoreServer;
using UnityEngine;
using UnityEngine.UI;

namespace Supyrb
{
	public class ExampleEchoClientUi : MonoBehaviour
	{
		[SerializeField] private ExampleEchoClient client = null;

		#region UiFields

		[Header("Settings Input")] [SerializeField]
		private InputField serverIpInput = null;

		[SerializeField] private InputField serverPortInput = null;

		[SerializeField] private Button sslConnectButton = null;

		[SerializeField] private Button tcpConnectButton = null;

		[SerializeField] private Button udpConnectButton = null;

		[SerializeField] private Button disconnectButton = null;

		[Header("Connection")] [SerializeField]
		private InputField messageInputField = null;

		[SerializeField] private Button sendMessageButton = null;

		[SerializeField] private Text serverResponseText = null;

		[SerializeField] private Text stateInfoText = null;

		[SerializeField] private Toggle sendAsyncToggle = null;

		#endregion

		private void Start()
		{
			tcpConnectButton.onClick.AddListener(TriggerTcpConnect);
			sslConnectButton.onClick.AddListener(TriggerSslConnect);
			udpConnectButton.onClick.AddListener(TriggerUdpConnect);
			disconnectButton.onClick.AddListener(TriggerDisconnect);
			sendMessageButton.onClick.AddListener(OnSendEcho);
			sendAsyncToggle.isOn = client.Async;
			sendAsyncToggle.onValueChanged.AddListener(OnAsyncToggleChanged);
		}

		public void UpdateState(IUnitySocketClient socketClient)
		{
			UpdateStateInfoText(socketClient);
			bool connected = socketClient != null && socketClient.IsConnected;
			sendMessageButton.interactable = connected;
			disconnectButton.interactable = connected;
			sslConnectButton.interactable = !connected;
			tcpConnectButton.interactable = !connected;
			udpConnectButton.interactable = !connected;
		}

		private void UpdateStateInfoText(IUnitySocketClient socketClient)
		{
			if (socketClient == null)
			{
				return;
			}

			var text = $"Server ip: {socketClient.Endpoint.Address}, Server port: {socketClient.Endpoint.Port}\n" +
			           $"Connecting: {socketClient.IsConnecting}, IsConnected: {socketClient.IsConnected}\n ";
			stateInfoText.text = text;
		}

		private void TriggerSslConnect()
		{
			var type = ExampleEchoClient.Type.Ssl;
			Connect(type);
		}

		private void TriggerTcpConnect()
		{
			var type = ExampleEchoClient.Type.Tcp;
			Connect(type);
		}

		private void TriggerUdpConnect()
		{
			var type = ExampleEchoClient.Type.Udp;
			Connect(type);
		}

		private void TriggerDisconnect()
		{
			client.Disconnect();
		}

		private void Connect(ExampleEchoClient.Type type)
		{
			var serverIp = serverIpInput.text;
			var serverPort = int.Parse(serverPortInput.text);
			client.ApplyInputAndConnect(serverIp, serverPort, type);
		}

		[ContextMenu("Send message")]
		private void OnSendEcho()
		{
			var message = Encoding.UTF8.GetBytes(messageInputField.text);
			client.SendEcho(message);
		}

		private void OnAsyncToggleChanged(bool async)
		{
			client.SetAsync(async);
		}

		public void AddResponseText(string messages)
		{
			serverResponseText.text = messages + serverResponseText.text;
		}
	}
}
