using System;
using ProtoBuf;

namespace VGn.CryptoSdk
{
	/// <summary>
	///  Свеча
	/// </summary>
	[Serializable]
	[ProtoContract]
	public class Candle : IHystoryData
	{
		[field: NonSerialized]
		public string Symbol { get; set; }
		/// <summary>
		///  Максимальная цена
		/// </summary>
		[ProtoMember(1)]
		[Newtonsoft.Json.JsonIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
		public float? HighValue { get; set; }

		public float High
		{
			get => HighValue ?? Close; 
			set => HighValue = value;
		}


		/// <summary>
		///  Минимальная цена
		/// </summary>
		[Newtonsoft.Json.JsonIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
		[ProtoMember(2)]
		public float? LowValue { get; set; }

		public float Low
		{
			get => LowValue ?? Close; 
			set => LowValue = value;
		}

		/// <summary>
		///  Цена открытия
		/// </summary>
		[Newtonsoft.Json.JsonIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
		[ProtoMember(3)]
		public float? OpenValue { get; set; }

		public float Open
		{
			get => OpenValue ?? Close; 
			set => OpenValue = value;
		}

		/// <summary>
		///  Цена закрытия 
		/// </summary>
		[ProtoMember(4)]
		public float Close { get; set; }

		/// <summary>
		///  Объём в количестве контрактов
		/// </summary>
		[Newtonsoft.Json.JsonIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
		[ProtoMember(5)]
		public long? VolumeValue { get; set; }

		public long Volume
		{
			get => VolumeValue ?? 0; 
			set => VolumeValue = value;
		}

		/// <summary>
		///  Количество сделок 
		/// </summary>
		[ProtoMember(6)]
		public long? Trades { get; set; }

		/// <summary>
		///  Время закрытия свечи
		/// </summary>
		[ProtoMember(7)]
		public DateTime Timestamp { get; set; }


		/// <summary>
		///  Завершённость свечи
		/// </summary>
		[field: NonSerialized]
		public CandleStates? State { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
		public float Price => Open;

	}
}