// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SslCertificateAsset.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace NetCoreServer
{
	[CreateAssetMenu(menuName = "SslCertificateAsset", fileName = "SslCertificateAsset", order = 0)]
	public class SslCertificateAsset : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField]
		private StreamingAssetPath certificatePath = null;

		[SerializeField]
		private string password = string.Empty;

		private SslProtocols protocols = SslProtocols.Tls12;

		private SslContext sslContext;

		public string GetCertPath()
		{
			return certificatePath.GetFullPath();
		}

		public SslContext GetContext()
		{
			if (sslContext == null)
			{
				X509Certificate2 cert = new X509Certificate2(GetCertPath(), password);
				sslContext = new SslContext(protocols, cert, OnValidationCallback);
			}

			return sslContext;
		}

		private bool OnValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
		{
			if (sslpolicyerrors == SslPolicyErrors.None)
			{
				return true;
			}

			Debug.LogWarning($"SSL Policy Errors: {sslpolicyerrors}", this);

			return true;
		}

		public void OnBeforeSerialize()
		{
			sslContext = null;
		}

		public void OnAfterDeserialize()
		{
			sslContext = null;
		}
	}
}
