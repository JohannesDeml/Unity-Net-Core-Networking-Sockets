// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamingAssetPath.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using UnityEngine;

namespace NetCoreServer
{
	[Serializable]
	public class StreamingAssetPath
	{
		[SerializeField]
		private string path;

		public StreamingAssetPath(string path)
		{
			this.path = path;
		}

		public string GetFullPath()
		{
			if (string.IsNullOrEmpty(path))
			{
				return string.Empty;
			}

			return Path.Combine(Application.streamingAssetsPath, path);
		}
	}
}
