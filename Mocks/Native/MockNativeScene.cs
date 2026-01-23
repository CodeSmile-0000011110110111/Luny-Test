using System;

namespace Luny.Test
{
	public sealed class MockNativeScene
	{
		public String Name { get; }
		public String Path { get; }

		public MockNativeScene(String name, String path)
		{
			Name = name;
			Path = path;
		}
	}
}
