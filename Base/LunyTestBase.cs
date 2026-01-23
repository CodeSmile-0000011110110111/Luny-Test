using NUnit.Framework;
using System;

namespace Luny.Test
{
	public abstract class LunyTestBase
	{
		public static ILunyEngineMockAdapter CreateEngineMockAdapter(Action<ILunyEngineMockAdapter> configure = null) =>
			MockEngineAdapter.Create(configure);

		[TearDown]
		public void TearDown()
		{
			if (LunyEngine.Instance != null)
			{
				LunyLogger.LogWarning($"Forcing {nameof(LunyEngine)} shutdown! Common cause: Test or Mock adapter threw an exception.");
				MockEngineAdapter.Teardown();
			}
		}
	}
}
