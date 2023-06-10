namespace VGn.CryptoSdk.Indicators;

public interface IIndicator
{
	/// <summary>
	/// Indicator name.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Whether the indicator is set.
	/// </summary>
	bool IsFormed { get; }

	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	void Reset();
}

/// <summary>
/// The interface describing indicator.
/// </summary>
public interface IIndicator<TInput, TOutput> : IIndicator
{
	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <returns>The new value of the indicator.</returns>
	TOutput Process(TInput input, bool isFinal = true);
}
