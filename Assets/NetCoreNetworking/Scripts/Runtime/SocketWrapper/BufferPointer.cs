// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BufferPointer.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace NetCoreServer
{
	public readonly struct BufferPointer
	{
		public readonly int Offset;
		public readonly int Length;

		public BufferPointer(int offset, int length)
		{
			Offset = offset;
			Length = length;
		}
	}
}
