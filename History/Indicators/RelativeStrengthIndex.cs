namespace VGn.CryptoSdk.Indicators;

using System.ComponentModel;

/// <summary>
/// Relative Strength Index.
/// </summary>
[DisplayName("RSI")]
public class RelativeStrengthIndex : RelativeStrengthIndex<WilderMovingAverage> { } 

/// <summary>
/// Relative Strength Index.
/// </summary>
[DisplayName("RSI")]
public class RelativeStrengthIndex<TMa> : LengthIndicator<float, float> 
	where TMa : LengthIndicator<float, float>, new()
{
	private readonly TMa _gain;
	private readonly TMa _loss;

	public RelativeStrengthIndex()
	{
		_gain = new TMa();
		_loss = new TMa();

		_name = $@"{nameof(RelativeStrengthIndex)}_{_gain.Name}";

		Length = 15;
	}


	public override bool IsFormed => _gain.IsFormed;


	public override void Reset()
	{
		_loss.Length = _gain.Length = Length;
	}

	
	private bool _isInitialized;
	private float _last;


	private float OnProcess(float input, bool isFinal)
	{
		if (!_isInitialized)
		{
			if (isFinal) 
				_isInitialized = true;

			return 0;
		}
		
		var delta = input - _last;

		var gainValue = _gain.Process(delta > 0 ? delta : 0, isFinal);
		var lossValue = _loss.Process(delta > 0 ? 0 : -delta, isFinal);

		if (lossValue == 0)
			return 100;

		var gainToLoss = gainValue / lossValue;
		if (gainToLoss < 0.00001f)
			return 0;

		return 100 - 100 / (1 + gainToLoss);
	}


	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <param name="isFinal"></param>
	/// <returns>The resulting value.</returns>
	public override float Process(float input, bool isFinal = true)
	{
		var result = OnProcess(input, isFinal);

		if (!isFinal) 
			return result;

		Value = result;
		_last = input;

		return result;
	}
}
