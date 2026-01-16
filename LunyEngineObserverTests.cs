using Luny.Engine;
using NUnit.Framework;
using System;
using System.Linq;

namespace Luny.Test
{
	[TestFixture]
	public sealed class LunyEngineObserverTests : LunyEngineTestBase
	{
		private readonly String[] _expectedMethodCallOrder =
		{
			nameof(ILunyEngineObserver.OnEngineStartup),
			nameof(ILunyEngineObserver.OnEnginePreUpdate),
			nameof(ILunyEngineObserver.OnEngineFixedStep),
			nameof(ILunyEngineObserver.OnEngineUpdate),
			nameof(ILunyEngineObserver.OnEngineLateUpdate),
			nameof(ILunyEngineObserver.OnEnginePostUpdate),
			nameof(ILunyEngineObserver.OnEngineShutdown),
		};

		private readonly String[] _repeatingMethods =
		{
			nameof(ILunyEngineObserver.OnEnginePreUpdate),
			nameof(ILunyEngineObserver.OnEngineFixedStep),
			nameof(ILunyEngineObserver.OnEngineUpdate),
			nameof(ILunyEngineObserver.OnEngineLateUpdate),
			nameof(ILunyEngineObserver.OnEnginePostUpdate),
		};

		[Test]
		public void Observer_LifecycleCallOrder_IsCorrect()
		{
			var adapter = CreateEngineMockAdapter(config =>
			{
				config.Iterations = 1;
				config.FixedStepRate = 60;
				config.UpdateRate = 60;
			});

			var observer = LunyEngine.Instance.GetObserver<LunyEngineMockObserver>();
			adapter.Run();

			// Filter out OnSceneLoaded/Unloaded if they were called (not expected here but good to be specific)
			var actualOrder = observer.CallOrder.Where(name => _expectedMethodCallOrder.Contains(name)).ToArray();
			Assert.That(actualOrder, Is.EqualTo(_expectedMethodCallOrder));

			// Verify once-only
			foreach (var methodName in _expectedMethodCallOrder)
			{
				Assert.That(observer.CallOrder.Count(name => name == methodName), Is.EqualTo(1),
					$"{methodName} expected to be called exactly once.");
			}
		}

		[Test]
		public void Observer_UpdateCallCount_IsCorrect()
		{
			var updateCount = 25;
			var adapter = CreateEngineMockAdapter(config =>
			{
				config.Iterations = updateCount;
				config.FixedStepRate = 60;
				config.UpdateRate = 60;
			});

			var observer = LunyEngine.Instance.GetObserver<LunyEngineMockObserver>();
			adapter.Run();

			// Verify count
			foreach (var methodName in _repeatingMethods)
			{
				Assert.That(observer.CallOrder.Count(name => name == methodName), Is.EqualTo(updateCount),
					$"{methodName} expected to be called exactly {updateCount} times.");
			}

			// Filter out OnSceneLoaded/Unloaded if they were called (not expected here but good to be specific)
			var actual = observer.CallOrder.Where(name => _repeatingMethods.Contains(name)).ToArray();
			string[] expected = Enumerable.Repeat(_repeatingMethods, updateCount).SelectMany(x => x).ToArray();
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}
