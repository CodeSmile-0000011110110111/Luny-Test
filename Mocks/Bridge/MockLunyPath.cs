using Luny.Engine.Bridge;
using System;

namespace Luny.Test
{
	public sealed class MockLunyPath : LunyPath
	{
		public MockLunyPath(String nativePath)
			: base(nativePath) {}

		protected override String ToEngineAgnosticPath(String nativePath) => NativePath;
	}
}
