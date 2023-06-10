using System;
using System.Runtime.Serialization;
//using System.Text.Json.Serialization;
using ProtoBuf;

namespace VGn.CryptoSdk
{
	[Serializable]
	[ProtoContract]
	//[JsonConverter(typeof(EmptyStringEnumConverter<Direction>))]
	public enum Direction : byte
	{
		/// <summary>
		///  Ничего не делай
		/// </summary>
		[ProtoEnum] [EnumMember] Wait = 0,
		/// <summary>
		///  Вставай в длинную позицию
		/// </summary>
		[ProtoEnum] [EnumMember] Buy = 1,
		/// <summary>
		///  Закрывай длинную позицию
		/// </summary>
		[ProtoEnum] [EnumMember] Sell = 2,

		/// <summary>
		///  Закрывай короткую позицию
		/// </summary>
		[ProtoEnum] [EnumMember] Cover = 3,
		
		/// <summary>
		///  Вставай в короткую позицию
		/// </summary>
		[ProtoEnum] [EnumMember] Short = 4,

		/// <summary>
		///  Закрывай все позиции
		/// </summary>
		[ProtoEnum] [EnumMember] GetOut = 5,

		//добавочные статусы результата торговли
		
		/// <summary>
		///  Частичная длинная позиция
		/// </summary>
		[ProtoEnum] [EnumMember] PartialBuy = 7,

		/// <summary>
		///  Частичная продажа
		/// </summary>
		[ProtoEnum] [EnumMember] PartialSell = 8,
		
		/// <summary>
		///  Частичная короткая позиция
		/// </summary>
		[ProtoEnum] [EnumMember] PartialShort = 9,

		/// <summary>
		///  Частичная откуп
		/// </summary>
		[ProtoEnum] [EnumMember] PartialCover = 10,

	}


	public static class DirectionHelper
	{
		public static bool IsSameSide(this Direction direction, Direction anotherDirection)
		{
			if (direction == anotherDirection)
				return true;

			if (direction == Direction.Wait 
			    || anotherDirection == Direction.Wait 
			    || direction == Direction.GetOut 
			    || anotherDirection == Direction.GetOut)
				return false;

			if ((int)direction % 2 == (int)anotherDirection % 2)
				return true;

			return false;



		}
	}
}
