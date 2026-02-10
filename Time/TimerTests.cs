using NUnit.Framework;

namespace Luny.Test.Time
{
	[TestFixture]
	public sealed class TimerTests
	{
		[Test]
		public void Elapses_After_Duration()
		{
			var timer = Timer.FromSeconds(1.0);
			var calls = 0;
			timer.OnElapsed += () => calls++;
			timer.Start();

			for (var i = 0; i < 9; i++)
				timer.Tick(0.1);

			Assert.That(calls, Is.EqualTo(0));

			timer.Tick(0.1);
			Assert.That(calls, Is.EqualTo(1));
			Assert.That(timer.IsRunning, Is.EqualTo(false));
		}

		[Test]
		public void AutoRepeat_Fires_Multiple_Times()
		{
			var timer = Timer.FromSeconds(0.25);
			var calls = 0;
			timer.OnElapsed += () => calls++;
			timer.AutoRepeat = true;
			timer.Start();

			for (var i = 0; i < 12; i++)
				timer.Tick(0.1);

			Assert.That(calls, Is.GreaterThanOrEqualTo(3));
			Assert.That(timer.IsRunning, Is.EqualTo(true));
		}

		[Test]
		public void IsRunning_Used_For_Pause_Resume()
		{
			var timer = Timer.FromSeconds(1.0);
			var calls = 0;
			timer.OnElapsed += () => calls++;
			timer.Start();

			for (var i = 0; i < 5; i++)
				timer.Tick(0.1);

			timer.Pause();
			for (var i = 0; i < 20; i++)
				timer.Tick(0.1);

			Assert.That(calls, Is.EqualTo(0));

			timer.Resume();
			for (var i = 0; i < 5; i++)
				timer.Tick(0.1);

			Assert.That(calls, Is.EqualTo(1));
		}

		[Test]
		public void TimeScale_Affects_Speed()
		{
			var timer = Timer.FromSeconds(1.0);
			var calls = 0;
			timer.OnElapsed += () => calls++;
			timer.TimeScale = 0.5; // half speed => takes twice as long
			timer.Start();

			for (var i = 0; i < 19; i++)
				timer.Tick(0.1);

			Assert.That(calls, Is.EqualTo(0));

			timer.Tick(0.1);
			Assert.That(calls, Is.EqualTo(1));
		}

		[Test]
		public void ToString_Default_Is_mm_ss()
		{
			var timer = Timer.FromSeconds(10.0);
			timer.Start();
			timer.Tick(1.0);
			var s = timer.ToString();
			Assert.That(s, Is.EqualTo("00:01"));
		}

		[Test]
		public void Remaining_Properties_Work()
		{
			var timer = Timer.FromSeconds(1.0);
			timer.Start();
			timer.Tick(0.4);

			Assert.That(timer.RemainingSeconds, Is.EqualTo(0.6).Within(1e-9));
			Assert.That(timer.RemainingMilliseconds, Is.EqualTo(600.0).Within(1e-6));
			Assert.That(timer.RemainingMinutes, Is.EqualTo(0.01).Within(1e-9));
		}

		[Test]
		public void Stop_Resets_Current()
		{
			var timer = Timer.FromSeconds(1.0);
			timer.Start();
			timer.Tick(0.5);
			Assert.That(timer.Current, Is.EqualTo(0.5).Within(1e-9));

			timer.Stop();
			Assert.That(timer.Current, Is.EqualTo(0.0));
			Assert.That(timer.IsRunning, Is.EqualTo(false));
		}
	}
}
