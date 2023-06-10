using System.ComponentModel;

namespace VGn.CryptoSdk.Indicators;

public abstract class BaseIndicator : IIndicator
{
	protected string _name;

	/// <summary>
	/// Indicator name.
	/// </summary>
	public string Name
	{
		get => _name ??= GetType().Name;
		set => _name = value;
	}
	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	public virtual void Reset()
	{
		IsFormed = false;
	}

	/// <summary>
	/// Whether the indicator is set.
	/// </summary>
	[Browsable(false)]
	public virtual bool IsFormed { get; protected set; }
}


/// <summary>
/// The base Indicator.
/// </summary>
public abstract class BaseIndicator<TInput, TOutput> : BaseIndicator, IIndicator<TInput, TOutput>
{
	public TOutput Value { get; set; }

	/// <summary>
	/// To handle the input value.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <param name="isFinal"></param>
	/// <returns>The resulting value.</returns>
	public abstract TOutput Process(TInput input, bool isFinal = true);

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	/// <returns>A string that represents the current object.</returns>
	public override string ToString()
	{
		return Name;
	}
}
