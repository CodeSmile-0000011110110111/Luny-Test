using NUnit.Framework;

namespace Luny.Test.Time
{
	[TestFixture]
	public sealed class AlarmTests
	{
		[Test]
		public void In_From_Produces_TargetTime()
		{
			var now = 10.0;
			var alarm = Alarm.In(3.333).From(now);
			Assert.That(alarm.Target, Is.EqualTo(now + 3.333).Within(1e-9));
		}

		[Test]
		public void At_Absolute_TargetTime()
		{
			var alarm = Alarm.At(42.0);
			Assert.That(alarm.Target, Is.EqualTo(42.0));
		}

		[Test]
		public void Comparisons_With_Double_Work_Both_Sides()
		{
			var now = 10.0;
			var alarm = Alarm.In(5.0).From(now); // target = 15
			Assert.That(now < alarm, Is.EqualTo(true));
			Assert.That(now <= alarm, Is.EqualTo(true));
			Assert.That(16.0 > alarm, Is.EqualTo(true));
			Assert.That(16.0 >= alarm, Is.EqualTo(true));
			Assert.That(15.0 == alarm, Is.EqualTo(true));
		}

		[Test]
		public void Remaining_And_IsElapsed_Work()
		{
			var now = 1.0;
			var alarm = Alarm.In(2.0).From(now); // target 3.0
			Assert.That(alarm.IsElapsed(2.5), Is.EqualTo(false));
			Assert.That(alarm.IsElapsed(3.0), Is.EqualTo(true));
			Assert.That(alarm.RemainingSeconds(2.0), Is.EqualTo(1.0).Within(1e-9));
			Assert.That(alarm.RemainingMilliseconds(2.0), Is.EqualTo(1000.0).Within(1e-6));
			Assert.That(alarm.RemainingMinutes(2.0), Is.EqualTo(1.0 / 60.0).Within(1e-12));
		}

		[Test]
		public void Unit_Factories_Are_Seconds_Based()
		{
			var ms = Alarm.InMilliseconds(500.0);
			var m = Alarm.InMinutes(0.5);
			var h = Alarm.InHours(0.001);
			Assert.That(ms.Target, Is.EqualTo(0.5).Within(1e-9));
			Assert.That(m.Target, Is.EqualTo(30.0).Within(1e-9));
			Assert.That(h.Target, Is.EqualTo(3.6).Within(1e-9));
		}
	}
}
