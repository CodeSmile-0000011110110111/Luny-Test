using NUnit.Framework;

namespace Luny.Test
{
	public abstract class LunyEngineTestBase
	{
		[TearDown]
		public void TearDown()
		{
			if (LunyEngine.Instance != null)
			{
				LunyLogger.LogWarning($"Forcing {nameof(LunyEngine)} shutdown! Common cause: Test or Mock adapter threw an exception.");
				LunyEngineMockAdapter.ForceShutdown();
			}
		}
	}
}
