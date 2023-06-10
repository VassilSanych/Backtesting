using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators
{
	[DisplayName("ATR")]
	public class AverageTrueRange: LengthIndicator<Candle, AtrSerie>
	{
		private readonly ExponentialMovingAverage _ma = new();
		private readonly TrueRange _tr = new();

		public AverageTrueRange()
		{
			_name = nameof(AverageTrueRange);
		}

		public override bool IsFormed => _ma.IsFormed;

		public override void Reset()
		{
			_ma.Length = Length;
			_tr.Reset();
			base.Reset();
		}


		public override AtrSerie Process(Candle input, bool isFinal = true)
		{
			var tr = _tr.Process(input, isFinal);
			var atr = _ma.Process(tr, isFinal);
			var output = new AtrSerie {TrueRange = tr, ATR = atr};
			if (isFinal)
				Value = output;
			return output;
		}
	}

	public class AtrSerie
	{
		public float TrueRange { get; set; }
		public float ATR { get; set; }
	}
}
