namespace VGn.CryptoSdk.Indicators;

using System.ComponentModel;

/// <summary>
/// Bollinger Bands.
/// </summary>
[DisplayName("BBands")]
public class BollingerBands : BollingerBands<SimpleMovingAverage> { }

/// <summary>
/// Bollinger Bands.
/// </summary>
[DisplayName("BBands")]
public class BollingerBands<TMa> : LengthIndicator<float, BollingerBandSerie<float>> 
	where TMa : LengthIndicator<float, float>, new()
{
	private readonly StandardDeviation _dev = new();
	private float _width;

	public BollingerBands()
		: this(new TMa())
	{}

	/// <summary>
	/// Initializes a new instance of BollingerBands.
	/// </summary>
	/// <param name="ma">Moving Average.</param>
	public BollingerBands(LengthIndicator<float, float> ma)
	{
		_name = $@"{nameof(BollingerBands)}_{ma.Name}";
		ma.Name = nameof(MovingAverage);
		MovingAverage = ma;
		Width = 2;
		Length = 10;
	}

	/// <summary>
	/// Middle line.
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<float, float> MovingAverage { get; }


	/// <summary>
	/// Bollinger Bands channel width. Default value equal to 2.
	/// </summary>
	public float Width
	{
		get => _width;
		set
		{
			_width = value;
			Reset();
		}
	}


	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	public override void Reset()
	{
		_dev.Length = MovingAverage.Length = Length;
	}


	/// <summary>
	/// Whether the indicator is set.
	/// </summary>
	public override bool IsFormed { get; protected set; }

	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <param name="isFinal">the candle is closed</param>
	/// <returns>The resulting value.</returns>
	public override BollingerBandSerie<float> Process(float input, bool isFinal = true)
	{
		var maValue = MovingAverage.Process(input, isFinal);
		var devValue = _dev.Process(input, isFinal);
		return Process(maValue, devValue, isFinal);
	}

	public BollingerBandSerie<float> Process(float ma, float deviation, bool isFinal = true)
	{
		var band = Width * deviation;
		var current = new BollingerBandSerie<float>
		{
			MovingAverage = ma,
			UpBand = ma + band,
			LowBand = ma - band,
			Deviation = deviation,
		};
		if (isFinal)
		{
			Value = current;
			IsFormed = true;
		}

		return current;
	}
}


public struct BollingerBandSerie<T>
{
	public T MovingAverage { get; set; }
	public T UpBand { get; set; }
	public T LowBand { get; set; }

	public T Deviation { get; set; }
}

