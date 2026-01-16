using Luny.Engine.Bridge;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;

namespace Luny.Test
{
	public class MockApplicationService : LunyApplicationServiceBase, ILunyApplicationService
	{
		public Boolean IsEditor => false;
		public Boolean IsPlaying => true;
		public void Quit(Int32 exitCode = 0) {}
	}

	public class MockDebugService : LunyDebugServiceBase, ILunyDebugService
	{
		public void LogInfo(String message) {}
		public void LogWarning(String message) {}
		public void LogError(String message) {}
		public void LogException(Exception exception) {}
	}

	public class MockEditorService : LunyEditorServiceBase, ILunyEditorService
	{
		public void PausePlayer() {}
	}

	public class MockSceneService : LunySceneServiceBase, ILunySceneService
	{
		public void ReloadScene() {}
		public IReadOnlyList<ILunyObject> GetAllObjects() => Array.Empty<ILunyObject>();
		public ILunyObject FindObjectByName(String name) => null;
	}

	public class MockTimeService : LunyTimeServiceBase, ILunyTimeService
	{
		public Int64 EngineFrameCount => 0;
		public Double ElapsedSeconds => 0;
	}
}
