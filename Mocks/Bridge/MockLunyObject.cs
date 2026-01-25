using Luny.Engine.Bridge;
using System;

namespace Luny.Test
{
	public sealed class MockLunyObject : LunyObject
	{
		public static ILunyObject ToLunyObject(MockNativeObject nativeObject)
		{
			var instanceId = nativeObject.ID;
			if (TryGetCached(instanceId, out var lunyObject))
				return lunyObject;

			return new MockLunyObject(nativeObject, instanceId);
		}

		private MockLunyObject(MockNativeObject mockObject, Int64 instanceId)
			: base(mockObject, instanceId, true, true) {}

		protected override String GetNativeObjectName() => Cast<MockNativeObject>().Name;
		protected override void DestroyNativeObject() => Cast<MockNativeObject>().Destroy();
		protected override Boolean IsNativeObjectValid() => NativeObject != null;
		protected override void SetNativeObjectName(String name) => Cast<MockNativeObject>().Name = name;
		protected override Boolean GetNativeObjectEnabledInHierarchy() => true;
		protected override Boolean GetNativeObjectEnabled() => true;
		protected override void SetNativeObjectEnabled() => Cast<MockNativeObject>().Enabled = true;
		protected override void SetNativeObjectDisabled() => Cast<MockNativeObject>().Enabled = false;
		protected override void SetNativeObjectVisible() => Cast<MockNativeObject>().Visible = true;
		protected override void SetNativeObjectInvisible() => Cast<MockNativeObject>().Visible = false;
	}
}
