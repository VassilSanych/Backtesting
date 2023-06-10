using System;
using ProtoBuf;

namespace VGn.CryptoSdk
{
	/// <summary>
	///  Сделка для хранения исторических данных
	/// </summary>
	[ProtoContract]
	[Serializable]
	public class HistoryTrade : IHystoryData
	{
		[field: NonSerialized] public string Symbol { get; set; }

		[ProtoMember(1)] public string Id { get; set; }

		[ProtoMember(2)] public DateTime Timestamp { get; set; }

		[ProtoMember(3)] public float Price { get; set; }

		[ProtoMember(4)] public uint Quantity { get; set; }

		[ProtoMember(5)] public Direction? Side { get; set; }

		[ProtoMember(6)] public OrderType? OrderType { get; set; }

		[ProtoMember(7)] public float? Rti { get; set; }
	}

	[ProtoContract]
	[Serializable]
	public enum OrderType : byte
	{
		[ProtoEnum] Fill,
		[ProtoEnum] Liquidation,
		[ProtoEnum] Assignment,
		[ProtoEnum] Termination
	}
}