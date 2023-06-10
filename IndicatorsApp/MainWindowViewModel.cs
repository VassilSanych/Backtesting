using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using VGn.CryptoSdk;
using VGn.CryptoSdk.Indicators;
using VGn.CryptoSdk.Interfaces;
using VGn.CryptoSdk.Interfaces.Enums;

namespace IndicatorsApp;


public struct IndicatorSerie<T>
{
	public T Value { get; set; }
	public DateTime Timestamp { get; set; }
}

	//public class Candle
	//{
	//	public TimestampTime Timestamp { get; set; }
	//	public float High { get; set; }
	//	public float Low { get; set; }
	//	public float Open { get; set; }
	//	public float Close { get; set; }
	//}

public class MainWindowViewModel : ObservableRecipient
{
	private readonly ILogger _logger;
	private readonly IHistoryStorage<Candle> _historyStorage;
	private readonly IMarket _market;

	public MainWindowViewModel(
		ILogger logger,
		IHistoryStorage<Candle> historyStorage,
		IMarket market)
	{
		_logger = logger;
		_market = market;
		_historyStorage = historyStorage;
		Candles = new ObservableCollection<Candle>(GetCandles());
	}


	public ObservableCollection<Candle> Candles { get; set; }

	private IEnumerable<IndicatorSerie<TOutput>> GetFromClose<TMa, TOutput>() 
		where TMa : BaseIndicator<float, TOutput>, new()
	{
		var ma = new TMa();
		foreach (var candle in Candles)
		{
			var value = ma.Process(candle.Close);
			if (ma.IsFormed)
				yield return new IndicatorSerie<TOutput>
				{
					Value = value,
					Timestamp = candle.Timestamp
				};
		}
	}


	private IEnumerable<IndicatorSerie<TOutput>> GetFromCandle<TIndicator, TOutput>() where TIndicator : BaseIndicator<Candle, TOutput>, new()
	{
		var ma = new TIndicator();
		foreach (var candle in Candles)
		{
			var value = ma.Process(candle);
			if (ma.IsFormed && value != null)
				yield return new IndicatorSerie<TOutput>
				{
					Value = value,
					Timestamp = candle.Timestamp
				};
		}
	}

	
	private IndicatorSerie<float>[]? _ema;

	public IndicatorSerie<float>[] Ema => 
		_ema ??= GetFromClose<ExponentialMovingAverage, float>().ToArray();


	private IndicatorSerie<float>[]? _smma;

	public IndicatorSerie<float>[] Smma => 
		_smma ??= GetFromClose<WilderMovingAverage, float>().ToArray();


	private IndicatorSerie<BollingerBandSerie<float>>[]? _bbands;
	public IndicatorSerie<BollingerBandSerie<float>>[] Bbands => 
		_bbands??= GetFromClose<BollingerBands, BollingerBandSerie<float>>().ToArray();


	private IndicatorSerie<AdxrSerie>[]? _adxr;

	public IndicatorSerie<AdxrSerie>[] Adxr => 
		_adxr ??= GetFromCandle<AverageDirectionalMovementRating, AdxrSerie>().ToArray();


	private IndicatorSerie<ParabolicSarSerie>[]? _psar;

	public IndicatorSerie<ParabolicSarSerie>[] Psar => 
		_psar ??= GetFromCandle<ParabolicSar, ParabolicSarSerie>().ToArray();


	private IndicatorSerie<float>[]? _rsi;

	public IndicatorSerie<float>[] Rsi => 
		_rsi ??= GetFromClose<RelativeStrengthIndex, float>().ToArray();


	public List<Candle> GetCandles()
	{
		var marketHistoryPath = Path.Combine(@"D:\CryptoHistory", _market.Name); //todo: content
		var utcTimeRange = new TimeRange(
			new DateTime(2022, 2, 1, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2022, 2, 5, 0, 0, 0, DateTimeKind.Utc));
		var timeframe = TimeframeType.M1;
		var legHistory = _historyStorage.FileIntoHistory(
				"PI_XBTUSD",
				marketHistoryPath,
				utcTimeRange,
				timeframe)
			.ToList();



		var purifiedLegHistory = _historyStorage.PurifyFileHistory(
			legHistory,
			utcTimeRange,
			timeframe);

		//private readonly IMarket _market;

		_logger.Trace(() =>
			$@"{_market}:PI_XBTUSD history size before {legHistory.Count} after  {purifiedLegHistory.Count}");

		return purifiedLegHistory;
	}

}


