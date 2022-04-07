namespace VGn.CryptoSdk.Indicators;

using System.ComponentModel;

/// <summary>
/// Simple moving average.
/// </summary>
[DisplayName("SMA")]
public class SimpleMovingAverage : BufferedLengthIndicator<float, float>
{
	public SimpleMovingAverage()
	{
		_name = nameof(SimpleMovingAverage);
		Length = 32;
	}


	private float _sum;

	public override void Reset()
	{
		_sum = 0;
		base.Reset();
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
			Buffer.Enqueue(input);
			_sum += input;

			if (IsFormed)
			{
				var value = Buffer.Dequeue();
				_sum -= value;
			}
			else
				_count++;

			Value = _sum / _count;
			return Value;
		}

		if (IsFormed)
		{
			var past = Buffer.Peek();
			return (_sum - past + input) / _count;
		}

		return (_sum + input) / (_count + 1);
	}
}
