using System.Collections.Generic;
using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators;

using System;

public class StandardDeviation: StandardDeviation<SimpleMovingAverage>{}


/// <summary>
/// Standard deviation.
/// </summary>
[DisplayName("StdDev")]
public class StandardDeviation<TMa> : BufferedLengthIndicator<float, float>
where TMa : LengthIndicator<float, float>, new() 
{
	private readonly TMa _ma;

	/// <summary>
	/// Initializes a new instance of the <see cref="StandardDeviation"/>.
	/// </summary>
	public StandardDeviation()
	{
		_ma = new TMa();
		_name = $@"{nameof(StandardDeviation)}_{_ma.Name}";
		
		Length = 10;
	}

	/// <summary>
	/// Whether the indicator is set.
	/// </summary>
	public override bool IsFormed => _ma.IsFormed;

	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	public override void Reset()
	{
		_ma.Length = Length;
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
		var maValue = _ma.Process(input);

		float sum = 0;
		float std;
		float current; 
		if (isFinal)
		{
			Buffer.Enqueue(input);

			if (IsFormed)
				Buffer.Dequeue();
			else
				_count++;

			foreach (var item in Buffer)
			{
				std = item - maValue;
				sum += std * std;
			}
			current = MathF.Sqrt(sum / _count);
			Value = current;
			return current;
		}

		int count;
		IEnumerable<float> buffer;
		if (IsFormed)
		{
			buffer = BufferSkip1();
			count = Length;
		}
		else
		{
			buffer = Buffer;
			count = _count + 1;
		}
		foreach (var item in buffer)
		{
			std = item - maValue;
			sum += std * std;
		}

		std = input - maValue;
		sum += std * std;
		current = MathF.Sqrt(sum / count);

		return current;
	}
}
