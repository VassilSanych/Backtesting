
namespace VGn.CryptoSdk.Indicators;

using System;
using System.Collections.Generic;

public abstract class BufferedLengthIndicator<TInput, TResult> : LengthIndicator<TInput, TResult>
{
	protected BufferedLengthIndicator()
	{
		Buffer = new Queue<TInput>();
	}

	/// <summary>
	/// The buffer for data storage.
	/// </summary>
	protected Queue<TInput> Buffer { get; }

	/// <summary>
	/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
	/// </summary>
	public override void Reset()
	{
		Buffer?.Clear();
		base.Reset();
	}

	protected IEnumerable<TInput> BufferSkip1()
	{
		using var e = Buffer.GetEnumerator();
		if (!e.MoveNext())
			yield break;
		while (e.MoveNext())
			yield return e.Current;
	}


}



/// <summary>
/// The base class for indicators with one resulting value and based on the period.
/// </summary>
/// <typeparam name="TResult">Result values type.</typeparam>
/// <typeparam name="TInput"></typeparam>
public abstract class LengthIndicator<TInput, TResult> : BaseIndicator<TInput, TResult>
{
	protected int _length = 1;

	/// <summary>
	/// Period length. By default equal to 1.
	/// </summary>
	public int Length
	{
		get => _length;
		set
		{
			if (value < 1)
				throw new ArgumentOutOfRangeException(nameof(value));

			_length = value;

			Reset();
		}
	}


	protected bool _isFormed;
	protected int _count;

	/// <summary>
	/// Whether the indicator is set.
	/// </summary>
	public override bool IsFormed
	{
		get => _isFormed || (_isFormed = _count >= Length);
		protected set => _isFormed = value;
	}


	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	/// <returns>A string that represents the current object.</returns>
	public override string ToString()
	{
		return $@"{Name} {Length}";
	}
}
