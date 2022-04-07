using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace VGn.CryptoSdk
{
	/// <summary>
	/// Candle states.
	/// </summary>
	[Serializable]
	public enum CandleStates : byte 
	{
		/// <summary>
		/// Empty state (candle doesn't exist).
		/// </summary>
		[EnumMember]
		None,

		/// <summary>
		/// Candle active.
		/// </summary>
		[EnumMember]
		Active,

		/// <summary>
		/// Candle finished.
		/// </summary>
		[EnumMember]
		Finished,
	}
}
