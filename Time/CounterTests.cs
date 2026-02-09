using NUnit.Framework;
using System;

namespace Luny.Test.Time
{
	[TestFixture]
	public sealed class CounterTests
	{
		[Test]
		public void Elapses_After_Target_Count()
		{
			var counter = new Luny.Counter(5);
			Int32 calls = 0;
			counter.OnElapsed += () => calls++;
			counter.Start();

			for (Int32 i = 0; i < 4; i++)
				counter.Increment();

			Assert.That(calls, Is.EqualTo(0));

			counter.Increment();
			Assert.That(calls, Is.EqualTo(1));
			Assert.That(counter.IsRunning, Is.EqualTo(false));
		}

		[Test]
		public void AutoRepeat_Cycles()
		{
			var counter = new Luny.Counter(3);
			Int32 calls = 0;
			counter.OnElapsed += () => calls++;
			counter.AutoRepeat = true;
			counter.Start();

			for (Int32 i = 0; i < 10; i++)
				counter.Increment();

			Assert.That(calls, Is.GreaterThanOrEqualTo(3));
			Assert.That(counter.IsRunning, Is.EqualTo(true));
		}

		[Test]
		public void Progress_And_Remaining()
		{
			var counter = new Luny.Counter(4);
			counter.Start();
			counter.Increment();
			Assert.That(counter.Progress, Is.EqualTo(0.25).Within(1e-9));
			Assert.That(counter.Remaining, Is.EqualTo(3));
		}
	}
}
