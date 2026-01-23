using Luny.Engine.Bridge;
using System;

namespace Luny.Test
{
	public sealed class MockLunyScene : LunyScene
	{
		public override String Name => Cast<MockNativeScene>().Name;

		public MockLunyScene(MockNativeScene nativeScene)
			: base(nativeScene, new MockLunyPath(nativeScene.Path)) {}
	}
}
