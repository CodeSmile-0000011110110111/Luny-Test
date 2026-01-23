using Luny.Engine.Services;
using System;
using System.Diagnostics;

namespace Luny.Test
{
	public sealed class MockDebugService : LunyDebugServiceBase, ILunyDebugService
	{
		public void LogInfo(String message) => Debug.WriteLine(message);
		public void LogWarning(String message) => Debug.WriteLine($"[Warn] {message}");
		public void LogError(String message) => Debug.WriteLine($"[ERROR] {message}");
		public void LogException(Exception exception) => Debug.WriteLine($"[EXCEPTION] {exception}");
	}
}
