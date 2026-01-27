using Luny.ContractTest;
using Luny.Engine;
using Luny.Engine.Bridge;
using Luny.Engine.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Luny.Test
{
	public abstract class EngineObserverTests : ContractTestBase
	{
		[Test]
		public void Observer_LifecycleCallOrder_IsCorrect()
		{
			String[] expectedMethodCallOrder =
			{
				nameof(ILunyEngineObserver.OnEngineStartup),
				nameof(ILunyEngineObserver.OnSceneLoaded),
				nameof(ILunyEngineObserver.OnEnginePreUpdate),
				nameof(ILunyEngineObserver.OnEngineFixedStep),
				nameof(ILunyEngineObserver.OnEngineUpdate),
				nameof(ILunyEngineObserver.OnEngineLateUpdate),
				nameof(ILunyEngineObserver.OnEnginePostUpdate),
				nameof(ILunyEngineObserver.OnEngineShutdown),
			};

			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			ShutdownEngine();

			// Filter out OnSceneLoaded/Unloaded
			var actualOrder = observer.CallOrder.Where(name => expectedMethodCallOrder.Contains(name)).ToArray();
			if (actualOrder.Length != expectedMethodCallOrder.Length)
				Console.WriteLine($"[DEBUG_LOG] Actual Call Order:\n{String.Join("\n", observer.CallOrder)}");
			Assert.That(actualOrder, Is.EqualTo(expectedMethodCallOrder));

			foreach (var methodName in expectedMethodCallOrder)
			{
				Assert.That(observer.CallOrder.Count(name => name == methodName), Is.EqualTo(1),
					$"{methodName} expected to be called exactly once.");
			}
		}

		[Test]
		public void Observer_UpdateCallCount_IsCorrect()
		{
			String[] repeatingMethods =
			{
				nameof(ILunyEngineObserver.OnEnginePreUpdate),
				nameof(ILunyEngineObserver.OnEngineFixedStep),
				nameof(ILunyEngineObserver.OnEngineUpdate),
				nameof(ILunyEngineObserver.OnEngineLateUpdate),
				nameof(ILunyEngineObserver.OnEnginePostUpdate),
			};

			var updateCount = 3;
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			SimulateFrames(updateCount);

			foreach (var methodName in repeatingMethods)
			{
				Assert.That(observer.RepeatingCallOrder.Count(name => name == methodName), Is.EqualTo(updateCount),
					$"[{Engine}] {methodName} expected to be called exactly {updateCount} times. Actual calls:\n" +
					$"{String.Join("\n", observer.RepeatingCallOrder)}");
			}
		}

		[Test]
		public void ObserverCallbacks_FrameCountInFirstFrame_IsOne()
		{
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			ShutdownEngine();

			var callbackNames = Enum.GetNames(typeof(MockEngineObserver.CallbackMethod));
			for (var i = 0; i < observer.FrameCounts.Length; i++)
			{
				var frameCount = observer.FrameCounts[i];
				Assert.That(frameCount, Is.EqualTo(1),
					$"[{Engine}] FrameCount is {frameCount} in {callbackNames[i]}, expected: 1. Actual calls: " +
					$"{String.Join("\n", observer.CallOrder)}");
			}
		}
	}

	internal sealed class MockEngineObserver : ILunyEngineObserver
	{
		public enum CallbackMethod
		{
			OnEngineStartup,
			OnEnginePreUpdate,
			OnEngineFixedStep,
			OnEngineUpdate,
			OnEngineLateUpdate,
			OnEnginePostUpdate,
			OnEngineShutdown,
		}

		private static readonly List<String> EngineCallbackNames = Enum.GetNames(typeof(CallbackMethod)).ToList();

		public List<String> CallOrder { get; } = new();
		public List<String> RepeatingCallOrder { get; } = new();
		public Int64[] FrameCounts { get; } = new Int64[EngineCallbackNames.Count];

		public void OnEngineStartup()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEnginePreUpdate()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			RepeatingCallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineFixedStep(Double fixedDeltaTime)
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			RepeatingCallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineUpdate(Double deltaTime)
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			RepeatingCallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineLateUpdate(Double deltaTime)
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			RepeatingCallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEnginePostUpdate()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			RepeatingCallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineShutdown()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnSceneLoaded(ILunyScene loadedScene) => CallOrder.Add(ILunyDebugService.GetMethodName());
		public void OnSceneUnloaded(ILunyScene unloadedScene) => CallOrder.Add(ILunyDebugService.GetMethodName());

		private Int32 GetEngineCallbackIndex([CallerMemberName] String methodName = "") => EngineCallbackNames.IndexOf(methodName);
	}

	[TestFixture]
	public sealed class GodotEngineObserverTests : EngineObserverTests
	{
		protected override NativeEngine Engine => NativeEngine.Godot;
	}

	[TestFixture]
	public sealed class UnityEngineObserverTests : EngineObserverTests
	{
		protected override NativeEngine Engine => NativeEngine.Unity;
	}
}
