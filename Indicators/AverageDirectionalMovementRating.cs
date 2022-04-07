using System.Collections.Generic;
using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators
{
	[DisplayName("ADXR")]
	public class AverageDirectionalMovementRating: AverageDirectionalMovementRating<WilderMovingAverage> {}


	[DisplayName("ADXR")]
	public class AverageDirectionalMovementRating<TMa> : LengthIndicator<Candle, AdxrSerie> 
		where TMa : LengthIndicator<float, float>, new()
	{
		private Queue<float> Buffer { get; }
		private readonly AverageDirectionalMovementIndex<TMa> _adx = new();

		public AverageDirectionalMovementRating()
		{
			_name = $"{nameof(AverageDirectionalMovementRating)}_{_adx.MaName}";
			Buffer = new Queue<float>();
			Length = 14;
		}

		public override void Reset()
		{
			_adx.Length = Length;
			base.Reset();
		}


		public override AdxrSerie Process(Candle input, bool isFinal = true)
		{
			var adx = _adx.Process(input, isFinal);

			AdxrSerie current;
			if (isFinal && _adx.IsFormed)
			{
				Buffer.Enqueue(adx.Adx);

				float value;
				if (IsFormed)
				{
					value = Buffer.Dequeue();
				}
				else
				{
					value = Buffer.Peek();
					_count++;
				}

				current = new AdxrSerie {Adx = adx, Adxr = (adx.Adx + value) / 2};
				Value = current;
				return Value;
			}


			Buffer.TryPeek(out var past);
			current = new AdxrSerie {Adx = adx, Adxr = (adx.Adx + past) / 2};
			return current;

		}
	}

	public struct AdxrSerie
	{
		public AdxSerie Adx { get; init; }
		public float Adxr { get; init; }

	}


}
