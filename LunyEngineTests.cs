using NUnit.Framework;

namespace Luny.Test
{
	[TestFixture]
	public sealed class LunyEngineTests : LunyTestBase
	{
		[Test] public void MandatoryServices_NotNull()
		{
			var adapter = CreateEngineMockAdapter(config => config.Iterations = 1);

			var engine = LunyEngine.Instance;
			Assert.That(engine, Is.Not.Null);

			Assert.That(engine.Application, Is.Not.Null);
			Assert.That(engine.Debug, Is.Not.Null);
			Assert.That(engine.Editor, Is.Not.Null);
			Assert.That(engine.Scene, Is.Not.Null);
			Assert.That(engine.Time, Is.Not.Null);
		}
	}
}
