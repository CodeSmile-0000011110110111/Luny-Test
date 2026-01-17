using NUnit.Framework;
using System;

namespace Luny.Test
{
	public abstract class LunyTestBase
	{
		internal static ILunyEngineMockAdapter CreateEngineMockAdapter(Action<ILunyEngineMockAdapter> configure = null) =>
			LunyEngineMockAdapter.Create(configure);

		[TearDown]
		public void TearDown()
		{
			if (LunyEngine.Instance != null)
			{
				LunyLogger.LogWarning($"Forcing {nameof(LunyEngine)} shutdown! Common cause: Test or Mock adapter threw an exception.");
				LunyEngineMockAdapter.Teardown();
			}
		}
	}
}
