using System;
using ProtoBuf;

namespace VGn.CryptoSdk
{
	[Serializable]
	[ProtoContract]
	public class OrderBookPart
	{
		[ProtoMember(1)]
		public double Price { get; set; }

		[ProtoMember(2)]
		public long Quantity { get; set; }

		[ProtoMember(3)]
		public long SumQuantity { get; set; }

		public override string ToString()
		{
			return $"{Price} ({Quantity})";
		}
	}
}
