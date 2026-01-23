using System;

namespace Luny.Test
{
	public sealed class MockNativeObject
	{
		private const Int64 StartNativeID = 1;
		private static Int64 s_NextNativeID = StartNativeID;

		private Boolean _isDestroyed;

		public Int64 ID { get; }
		public String Name { get; set; }
		public String Type { get; set; }
		public Boolean Enabled { get; set; }
		public Boolean Visible { get; set; }

		public MockNativeObject(String name, String type = null)
		{
			ID = s_NextNativeID++;
			Name = name;
			Type = type;
		}

		public void Destroy()
		{
			if (!_isDestroyed)
			{
				_isDestroyed = true;
				LunyLogger.LogInfo($"Destroyed {nameof(MockNativeObject)}: {this}");
			}
		}

		public override String ToString() => $"{Name} ({ID})";
	}
}
