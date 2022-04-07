using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace VGn.CryptoSdk
{
	[Serializable]
	[ProtoContract]
	public class OrderBook
	{
		[field: NonSerialized]
		public string Symbol { get; set; }

		[field: NonSerialized]
		public OrderBookSort AskSorting { get; set; } = OrderBookSort.Increase;

		[field: NonSerialized]
		public OrderBookSort BidSorting { get; set; } = OrderBookSort.Decrease;

		[ProtoMember(1)]
		public List<OrderBookPart> Bids { get; set; }

		[ProtoMember(2)]
		public List<OrderBookPart> Asks { get; set; }
		
		[ProtoMember(3)]
		public DateTime Timestamp { get; set; }


		public double GetSpread()
		{
			if (!Asks.Any() || !Bids.Any())
				return 0;
			var result = Asks.First().Price - Bids.First().Price;
			//var result = Asks.Min(x => x.Price) - Bids.Max(x => x.Price);
			return result;
		}


		/// <summary>
		///  Заполнение бидов отсортированными данными
		/// </summary>
		/// <param name="bids"></param>
		public void ReplaceBids(IEnumerable<OrderBookPart> bids)
		{
			Bids ??= new List<OrderBookPart>();
			Replace(Bids, bids, BidSorting);
		}


		/// <summary>
		///  Заполнение асков отсортированными данными
		/// </summary>
		/// <param name="asks"></param>
		public void ReplaceAsk(IEnumerable<OrderBookPart> asks)
		{
			Asks ??= new List<OrderBookPart>();
			Replace(Asks, asks, AskSorting);
		}


		/// <summary>
		///  Заполнение коллекции заявок отсортированными данными
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="data"></param>
		/// <param name="sorting"></param>
		private void Replace(List<OrderBookPart> collection, IEnumerable<OrderBookPart> data, OrderBookSort sorting)
		{
			if (collection == null || data == null)
				return;
			collection.Clear();
			collection.AddRange(data);

			IComparer<OrderBookPart> comparer;
			switch (sorting)
			{
				case OrderBookSort.Increase:
					comparer = new OrderBookPartComparerIncrease();
					break;

				case OrderBookSort.Decrease:
					comparer = new OrderBookPartComparerDecrease();
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(sorting), sorting, null);
			}
			collection.Sort(comparer);
		}
	}

	internal class OrderBookPartComparerIncrease: IComparer<OrderBookPart>
	{
		public int Compare(OrderBookPart x, OrderBookPart y)
		{
			if (x == null && y == null)
				return 0;
			if (y == null)
				return +1;
			if (x == null)
				return -1;
			return x.Price.CompareTo(y.Price);
		}
	}

	internal class  OrderBookPartComparerDecrease: IComparer<OrderBookPart>
	{
		public int Compare(OrderBookPart x, OrderBookPart y)
		{
			if (x == null && y == null)
				return 0;
			if (y == null)
				return -1;
			if (x == null)
				return +1;
			return y.Price.CompareTo(x.Price);
		}
	}


	[ProtoContract]
	public enum OrderBookType
	{
		[ProtoEnum] Buy,
		[ProtoEnum] Sell,
		[ProtoEnum] Both
	}

	[ProtoContract]
	public enum OrderBookSort
	{
		[ProtoEnum] Increase,
		[ProtoEnum] Decrease
	}
}