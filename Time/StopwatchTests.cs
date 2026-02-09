using NUnit.Framework;

namespace Luny.Test.Time
{
	[TestFixture]
	public sealed class StopwatchTests
	{
		[Test]
		public void Elapsed_Is_Difference()
		{
			var sw = Luny.Stopwatch.Start(10.0);
			Assert.That(sw.ElapsedSeconds(10.0), Is.EqualTo(0.0).Within(1e-9));
			Assert.That(sw.ElapsedSeconds(12.5), Is.EqualTo(2.5).Within(1e-9));
		}

		[Test]
		public void Unit_Conversions_Work()
		{
			var sw = Luny.Stopwatch.Start(10.0);
			Assert.That(sw.ElapsedMilliseconds(10.25), Is.EqualTo(250.0).Within(1e-6));
			Assert.That(sw.ElapsedMinutes(70.0), Is.EqualTo(1.0).Within(1e-9));
		}
	}
}
