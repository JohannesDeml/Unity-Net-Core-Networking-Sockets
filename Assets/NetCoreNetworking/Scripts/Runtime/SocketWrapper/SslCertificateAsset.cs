// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SslCertificateAsset.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
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
		private ByteFileAsset certificateBytesAsset = null;

		[SerializeField]
		private string password = string.Empty;

		private SslProtocols protocols = SslProtocols.Tls12;

		private SslContext sslContext;

		public SslContext GetContext()
		{
			if (sslContext == null)
			{
				var certBytes = certificateBytesAsset.Bytes;
				try
				{
					X509Certificate2 cert = new X509Certificate2(certBytes, password);
					sslContext = new SslContext(protocols, cert, OnValidationCallback);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
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
