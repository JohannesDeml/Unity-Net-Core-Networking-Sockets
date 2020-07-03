// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteFileAsset.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace NetCoreServer
{
	[Serializable]
	public class ByteFileAsset
	{
		[SerializeField]
		private string guid = null;

		[SerializeField]
		private byte[] bytes = null;

		public string Guid => guid;
		public byte[] Bytes => bytes;

		public ByteFileAsset()
		{
		}
	}
}
