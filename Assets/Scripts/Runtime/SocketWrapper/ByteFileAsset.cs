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
		private string guid;

		[SerializeField]
		private byte[] bytes;

		public byte[] Bytes => bytes;

		public ByteFileAsset()
		{
		}
	}
}
