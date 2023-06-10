using System;
using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators
{
	[DisplayName("ADX")]
	public class AverageDirectionalMovementIndex: AverageDirectionalMovementIndex<WilderMovingAverage> {}

	[DisplayName("ADX")]
	public class AverageDirectionalMovementIndex<TMa> :LengthIndicator<Candle, AdxSerie>
	where TMa : LengthIndicator<float, float>, new() 
	{
		private readonly TrueRange _tr = new ();
		private readonly TMa _trMa = new ();
		private readonly TMa _dxMa = new ();
		private readonly TMa _diPlusMa = new ();
		private readonly TMa _diMinusMa = new ();

		private Candle _prev;

		public AverageDirectionalMovementIndex()
		{
			_name = $@"{nameof(AverageDirectionalMovementIndex)}_{_dxMa.Name}";
			Length = 14;
		}

		public string MaName => _dxMa.Name;

		public override void Reset()
		{
			_trMa.Length = _dxMa.Length = _diMinusMa.Length = _diPlusMa.Length = Length;
			base.Reset();
		}

		public override bool IsFormed => _dxMa.IsFormed;

		public override AdxSerie Process(Candle input, bool isFinal = true)
		{
			if (_prev == null)
			{
				if (isFinal) 
					_prev = input;

				return default;
			}

			var plusDm = input.High - _prev.High;
			var minusDm = _prev.Low - input.Low;

			if (plusDm < 0) 
				plusDm = 0;

			if (minusDm < 0) 
				minusDm = 0;

			if (plusDm == minusDm)
			{
				plusDm = 0;
				minusDm = 0;
			}
			else if (plusDm < minusDm)
				plusDm = 0;
			else
				minusDm = 0;

			var tr = _tr.Process(input, isFinal);
			var trMa = _trMa.Process(tr, isFinal);

			float diPlus;
			float diMinus;
			float dx;
			if (trMa == 0)
			{
				diPlus = 0;
				diMinus = 0;
				dx = 0;
			}
			else
			{
				diPlus = _diPlusMa.Process(plusDm, isFinal) * 100 / trMa;
				diMinus = _diMinusMa.Process(minusDm, isFinal) * 100 / trMa;
				dx = 100 * Math.Abs(diPlus - diMinus) / (diPlus + diMinus);
			}


			//float diPlus;
			//float diMinus;
			//float dx;
			//if (tr == 0)
			//{
			//	diPlus = 0;
			//	diMinus = 0;
			//	dx = 0;
			//}
			//else
			//{
			//	diPlus = _diPlusMa.Process(plusDm / tr, isFinal);
			//	diMinus = _diMinusMa.Process(minusDm / tr, isFinal);
			//	dx = 100 * Math.Abs(diPlus - diMinus) / (diPlus + diMinus);
			//}




			var adx = _diPlusMa.IsFormed ? _dxMa.Process(dx, isFinal) : 0;

			var result = new AdxSerie
			{
				Adx = adx, 
				Dx = dx,
				TrueRange = trMa, 
				DiPlus = diPlus, 
				DiMinus = diMinus
			};

			if (isFinal)
			{
				Value = result;
				_prev = input;
			}

			return result;
		}
	}

	public struct AdxSerie
	{
		public float TrueRange { get; init; }
		public float DiPlus { get; init; }
		public float DiMinus { get; init; }
		public float Dx { get; init; }
		public float Adx { get; init; }
	}
}
