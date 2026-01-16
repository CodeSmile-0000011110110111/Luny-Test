using Luny.Engine;
using System;

// ReSharper disable NotNullOrRequiredMemberIsNotInitialized

namespace Luny.Test
{
	/// <summary>
	/// Mock engine adapter for unit testing LunyEngine lifecycle.
	/// </summary>
	/// <remarks>
	///	The assumption is that FixedStep always runs in the first frame.
	/// </remarks>
	internal sealed class LunyEngineMockAdapter : ILunyEngineNativeAdapter
	{
		public event Action<Int32> OnEndOfFrame;

		public const Int32 DefaultIterations = 10;
		public const Int32 DefaultUpdateRate = 60;
		public const Int32 DefaultFixedStepRate = 25;

		private static ILunyEngineNativeAdapter s_Instance;
		private ILunyEngineAdapter _lunyEngine;

		public Int32 Iterations { get; set; } = DefaultIterations;
		public Int32 UpdateRate { get; set; } = DefaultUpdateRate;
		public Int32 FixedStepRate { get; set; } = DefaultFixedStepRate;

		public Int32 UpdateCallCount { get; private set; }
		public Int32 FixedStepCallCount { get; private set; }

		internal static void ForceShutdown() => ((LunyEngineMockAdapter)s_Instance)?.Shutdown();

		public LunyEngineMockAdapter() => Initialize();

		private void Initialize()
		{
			LunyTraceLogger.LogInfoInitializing(this);
			s_Instance = ILunyEngineNativeAdapter.ValidateAdapterSingletonInstance(s_Instance, this);
			_lunyEngine = LunyEngine.CreateInstance(this);
			LunyTraceLogger.LogInfoInitialized(this);
		}

		public void Run()
		{
			VerifyMockAdapterParameters();

			Startup();

			var deltaTime = 1.0 / UpdateRate;
			var fixedDeltaTime = 1.0 / FixedStepRate;
			var fixedStepAccumulator = 0.0;

			for (var frameCount = 1; frameCount <= Iterations; frameCount++)
			{
				fixedStepAccumulator += deltaTime;
				while (fixedStepAccumulator > 0.0)
				{
					LunyLogger.LogInfo($"Mock FixedStep: {1000 * fixedDeltaTime:0.#}ms");
					FixedStepCallCount++;
					FixedStep(fixedDeltaTime);
					fixedStepAccumulator -= fixedDeltaTime;
				}

				LunyLogger.LogInfo($"Mock Update: {1000 * deltaTime:0.#}ms");
				UpdateCallCount++;
				Update(deltaTime);

				OnEndOfFrame?.Invoke(frameCount);
			}

			Shutdown();
		}

		private void Startup()
		{
			ILunyEngineNativeAdapter.ThrowIfAdapterNull(s_Instance);
			ILunyEngineNativeAdapter.ThrowIfLunyEngineNull(_lunyEngine);
			_lunyEngine.OnEngineStartup();
		}

		private void FixedStep(Double delta) => _lunyEngine.OnEngineFixedStep(delta);

		private void Update(Double delta)
		{
			_lunyEngine.OnEngineUpdate(delta);
			_lunyEngine.OnEngineLateUpdate(delta);
		}

		internal void Shutdown()
		{
			if (s_Instance == null)
				return;

			try
			{
				ILunyEngineNativeAdapter.Shutdown(s_Instance, _lunyEngine);
			}
			catch (Exception ex)
			{
				LunyLogger.LogException(ex);
			}
			finally
			{
				ILunyEngineNativeAdapter.ShutdownComplete(s_Instance);
				LunyEngine.ResetDisposedFlag_UnityEditorAndUnitTestsOnly();
				_lunyEngine = null;
				s_Instance = null;
			}
		}

		private void VerifyMockAdapterParameters()
		{
			if (Iterations <= 0)
				throw new ArgumentOutOfRangeException(nameof(Iterations), $"{nameof(Iterations)} must be >= 1, is: {Iterations}");
			if (UpdateRate <= 0)
				throw new ArgumentOutOfRangeException(nameof(UpdateRate), $"{nameof(UpdateRate)} must be > 0, is: {UpdateRate}");
			if (FixedStepRate <= 0)
				throw new ArgumentOutOfRangeException(nameof(FixedStepRate), $"{nameof(FixedStepRate)} must be > 0, is: {FixedStepRate}");

			LunyLogger.LogInfo($"Test Parameters => Iterations: {Iterations}, UpdateRate: {UpdateRate}, FixedStepRate: {FixedStepRate}", this);
		}
	}
}
