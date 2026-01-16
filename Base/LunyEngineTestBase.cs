using NUnit.Framework;
using System;

namespace Luny.Test
{
	public abstract class LunyEngineTestBase
	{
		internal static ILunyEngineMockAdapter CreateEngineMockAdapter(Action<ILunyEngineMockAdapter> configure = null) =>
			LunyEngineMockAdapter.Create(configure);

		[TearDown]
		public void TearDown()
		{
			if (LunyEngineInternal.Instance != null)
			{
				LunyLogger.LogWarning($"Forcing {nameof(LunyEngineInternal)} shutdown! Common cause: Test or Mock adapter threw an exception.");
				LunyEngineMockAdapter.Teardown();
			}
		}
	}
}
