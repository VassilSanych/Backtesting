using System;
using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators;

/// <summary>
/// True range.
/// </summary>
[DisplayName("TR")]
public class TrueRange : BaseIndicator<Candle, float>
{
	private Candle _prevCandle;

	public TrueRange()
	{
		_name = nameof(TrueRange);
	}

	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		_prevCandle = null;
	}
	
	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="candle">The input value.</param>
	/// <param name="isFinal"></param>
	/// <returns>The resulting value.</returns>
	public override float Process(Candle candle, bool isFinal = true)
	{
		float tr;
		if (_prevCandle != null)
		{
			tr = Math.Max(_prevCandle.Close, candle.High)
			     - Math.Min(_prevCandle.Close, candle.Low);

			if (isFinal)
			{
				IsFormed = true;
				_prevCandle = candle;
				Value = tr;
			}

			return tr;
		}
		
		tr = candle.High - candle.Low;

		if (isFinal)
		{
			_prevCandle = candle;
			Value = tr;
		}

		return tr;
	}
}
