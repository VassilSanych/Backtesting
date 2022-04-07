using System;

namespace VGn.CryptoSdk
{
	public interface IHystoryData
	{
		DateTime Timestamp { get; set; }

		float Price { get; }
	}
}
