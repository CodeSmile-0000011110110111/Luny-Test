using Luny.Engine;
using Luny.Engine.Bridge;
using System;
using System.Collections.Generic;

namespace Luny.Test
{
	public sealed class LunyEngineMockObserver : ILunyEngineObserver
	{
		public List<String> CallOrder { get; } = new();

		public void OnEngineStartup() => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnEnginePreUpdate() => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnEngineFixedStep(Double fixedDeltaTime) => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnEngineUpdate(Double deltaTime) => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnEngineLateUpdate(Double deltaTime) => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnEnginePostUpdate() => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnEngineShutdown() => CallOrder.Add(Helper.GetCurrentMethodName());

		public void OnSceneLoaded(ILunyScene loadedScene) => CallOrder.Add(Helper.GetCurrentMethodName());
		public void OnSceneUnloaded(ILunyScene unloadedScene) => CallOrder.Add(Helper.GetCurrentMethodName());
	}
}
