using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VGn.CryptoSdk.Indicators;

namespace Indicators.Tests
{
	[TestClass]
	public class SmaTests
    {
	    [TestMethod]
	    public void IndicatorTest()
	    {
		    var indicator = new SimpleMovingAverage {Length = 3};
		    indicator.Process(1f);
		    indicator.Process(2f);
		    indicator.Process(3f);
		    indicator.Process(4f);
		    var last = indicator.Process(5f);

		    var result = indicator.Value;
			Console.WriteLine(result);
			Assert.AreEqual(4, result);
			Assert.AreEqual(last, result);
	    }
	}
}
