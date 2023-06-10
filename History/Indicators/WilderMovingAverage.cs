using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators;


/// <summary>
/// Welles Wilder Moving Average. (SMMA)
/// </summary>
[DisplayName("WilderMA")] //[DisplayName("SMMA")]
public class WilderMovingAverage : LengthIndicator<float, float>
{
	public WilderMovingAverage()
	{
		_name = nameof(WilderMovingAverage);
		Length = 32;
	}

	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <param name="isFinal"></param>
	/// <returns>The resulting value.</returns>
	public override float Process(float input, bool isFinal = true)
	{
		if (isFinal)
		{
			if (!IsFormed)
				_count++;
			
			Value = (Value * (_count - 1) + input)/_count;	
			return Value;
		}

		var count = IsFormed ? Length : _count + 1;
		return (Value * (count - 1) + input)/count;
	}
}
