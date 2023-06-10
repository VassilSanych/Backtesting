using System;
using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators;

/// <summary>
/// Trend indicator implementation - Parabolic SAR.
/// </summary>
/// <remarks>
/// http://ta.mql4.com/indicators/trends/parabolic_sar.
/// </remarks>
[DisplayName("PSAR")]
public class ParabolicSar : BaseIndicator<Candle, ParabolicSarSerie>
{
	public ParabolicSar()
	{
		_name = nameof(ParabolicSar);
		Acceleration = 0.02f;
		AccelerationStep = 0.02f;
		AccelerationMax = 0.2f;
	}

	public override void Reset()
	{
		base.Reset();
		_prev = null;
		_prev1 = null;
	}


	/// <summary>
	/// Acceleration factor.
	/// </summary>
	public float Acceleration { get; set; }

	/// <summary>
	/// Acceleration factor step.
	/// </summary>
	public float AccelerationStep { get; set; }

	/// <summary>
	/// Maximum acceleration factor.
	/// </summary>
	public float AccelerationMax { get; set; }

	private Candle _prev;
	private Candle _prev1;


	private float _accelerationFactor;
	private float _extremePoint;
	private float _priorSar;
	private bool _isRising; // initial guess

	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="candle">The input value.</param>
	/// <param name="isFinal"></param>
	/// <returns>The resulting value.</returns>
	public override ParabolicSarSerie Process(Candle candle, bool isFinal = true)
	{
		ParabolicSarSerie current = default;
		if (_prev == null)
		{
			if (isFinal)
			{
				_prev = candle;
				_extremePoint = candle.High;
				_accelerationFactor = Acceleration;
				_priorSar = candle.Low;
				_isRising = true;
				Value = current;
			}

			return current;
		}

		// was rising
		if (_isRising)
		{
			var sar = _priorSar + _accelerationFactor * (_extremePoint - _priorSar);

			// SAR cannot be higher than last two lows
			if (_prev1 != null)
				sar = Math.Min(sar, Math.Min(_prev.Low, _prev1.Low));

			// turn down
			if (candle.Low < sar)
			{
				current  = new()
				{
					Sar = _extremePoint,
					IsReversal = true,
					IsRising = false
				};

				if (isFinal)
				{
					_isRising = false;
					IsFormed = true;
					_accelerationFactor = Acceleration;
					_extremePoint = candle.Low;
					Value = current;
				}
			}

			// continue rising
			else
			{
					current = new()
					{
						Sar = sar,
						IsReversal = false,
						IsRising = true
					};
					if (isFinal) 
						Value = current;

				// new high extreme point
				if (isFinal && candle.High > _extremePoint)
				{
					_extremePoint = candle.High;
					_accelerationFactor = Math.Min(
						_accelerationFactor += AccelerationStep,
						AccelerationMax);
				}
			}
		}

		// was falling
		else
		{
			var sar = _priorSar - _accelerationFactor * (_priorSar - _extremePoint);

			// SAR cannot be lower than last two highs
			if (_prev1 != null)
				sar = Math.Max(sar, Math.Max(_prev.High, _prev1.High));

			// turn up
			if (candle.High > sar)
			{
				current = new()
				{
					Sar = _extremePoint,
					IsReversal = true,
					IsRising = true
				};

				if (isFinal)
				{
					_isRising = true;
					IsFormed = true;
					_accelerationFactor = Acceleration;
					_extremePoint = candle.High;
					Value = current;
				}
			}

			// continue falling
			else
			{
				current = new()
				{
					Sar = sar,
					IsReversal = false,
					IsRising = false
				};

				if (isFinal)
					Value = current;

				// new low extreme point
				if (isFinal && candle.Low < _extremePoint)
				{
					_extremePoint = candle.Low;
					_accelerationFactor = Math.Min(
							_accelerationFactor += AccelerationStep,
							AccelerationMax);
				}
			}
		}

		if (isFinal)
		{
			_prev1 = _prev;
			_prev = candle;
			_priorSar = current.Sar;
		}

		return current;
	}
}

public struct ParabolicSarSerie
{
	public float Sar { get; init; }
	public bool IsReversal { get; init; }
	public bool IsRising { get; init; }
}

