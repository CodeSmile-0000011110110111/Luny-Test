using NUnit.Framework;
using System;

namespace Luny.Test
{
	[TestFixture]
	public sealed class LunyEngineMockAdapterTests : LunyTestBase
	{
		[Test] public void Mock_MinimumParameters_UpdateAndFixedStepRunOnce()
		{
			var adapter = CreateEngineMockAdapter(config =>
			{
				config.Iterations = 1;
				config.FixedStepRate = 1;
				config.UpdateRate = 1;
			});

			Assert.DoesNotThrow(() => adapter.Run());
			Assert.That(adapter.FixedStepCallCount, Is.EqualTo(1));
			Assert.That(adapter.UpdateCallCount, Is.EqualTo(1));
		}

		[Test] public void Mock_InvalidIterations_Throws()
		{
			var adapter = CreateEngineMockAdapter(config => config.Iterations = 0);
			Assert.Throws<ArgumentOutOfRangeException>(() => adapter.Run());
		}

		[Test] public void Mock_InvalidFixedStepRate_Throws()
		{
			var adapter = CreateEngineMockAdapter(config => { config.FixedStepRate = 0; });
			Assert.Throws<ArgumentOutOfRangeException>(() => adapter.Run());
		}

		[Test] public void Mock_InvalidUpdateRate_Throws()
		{
			var adapter = CreateEngineMockAdapter(config => { config.UpdateRate = 0; });
			Assert.Throws<ArgumentOutOfRangeException>(() => adapter.Run());
		}

		// Documents at which FixedStep rate our mock adapter's FixedStep call count increments.
		// This may deviate for any engine depending on how they determine when to call FixedStep.
		// Assumption: FixedStep always runs in the first frame.
		[TestCase(10, 240, 41)]
		[TestCase(10, 239, 40)]
		[TestCase(10, 121, 21)]
		[TestCase(10, 120, 20)]
		[TestCase(10, 61, 11)]
		[TestCase(10, 60, 10)]
		[TestCase(10, 54, 10)]
		[TestCase(10, 53, 9)]
		[TestCase(10, 48, 9)]
		[TestCase(10, 47, 8)]
		[TestCase(10, 42, 8)]
		[TestCase(10, 41, 7)]
		[TestCase(10, 36, 7)]
		[TestCase(10, 35, 6)]
		[TestCase(10, 31, 6)]
		[TestCase(10, 30, 5)]
		[TestCase(10, 24, 5)]
		[TestCase(10, 23, 4)]
		[TestCase(10, 18, 4)]
		[TestCase(10, 17, 3)]
		[TestCase(10, 13, 3)]
		[TestCase(10, 12, 2)]
		[TestCase(10, 7, 2)]
		[TestCase(10, 6, 1)]
		[TestCase(10, 1, 1)]
		public void Mock_FixedStepRate_ExpectedCallCount(Int32 iterations, Int32 stepRate, Int32 expectedStepCount)
		{
			var adapter = CreateEngineMockAdapter(config =>
			{
				config.Iterations = iterations;
				config.FixedStepRate = stepRate;
			});

			var iteration = 0;
			adapter.OnEndOfFrame += frameCount =>
			{
				iteration++;
				Assert.That(frameCount, Is.EqualTo(iteration));
			};

			Assert.DoesNotThrow(() => adapter.Run());
			Assert.That(adapter.FixedStepCallCount, Is.EqualTo(expectedStepCount));
			Assert.That(adapter.UpdateCallCount, Is.EqualTo(iterations));
		}
	}
}
