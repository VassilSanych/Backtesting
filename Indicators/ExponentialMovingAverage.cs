using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators;

/// <summary>
/// Exponential Moving Average.
/// </summary>
[DisplayName("EMA")]
public class ExponentialMovingAverage : LengthIndicator<float, float>
{
	private float _multiplier = 1;


	/// <summary>
	/// Initializes a new instance of the <see cref="ExponentialMovingAverage"/>.
	/// </summary>
	public ExponentialMovingAverage()
	{
		_name = nameof(ExponentialMovingAverage);
		Length = 32;
	}


	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		_multiplier = 2f / (Length + 1);
	}


	public override bool IsFormed { get; protected set; }


	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <param name="isFinal"></param>
	/// <returns>The resulting value.</returns>
	public override float Process(float input, bool isFinal = true)
	{
		if (!IsFormed)
		{
			if (isFinal)
			{
				Value = input;
				IsFormed = true;
			}
			return input;
		}

		var curValue = (input - Value) * _multiplier + Value;

		if (isFinal)
			Value = curValue;

		return curValue;
	}
}
