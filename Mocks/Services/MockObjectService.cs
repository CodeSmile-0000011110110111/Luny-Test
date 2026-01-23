using Luny.Engine.Bridge;
using Luny.Engine.Bridge.Enums;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;

namespace Luny.Test
{
	public sealed class MockObjectService : LunyObjectServiceBase, ILunyObjectService
	{
		public List<String> Log { get; } = new();

		public ILunyObject CreateEmpty(String name)
		{
			Log.Add($"{nameof(CreateEmpty)}({name})");
			return new MockLunyObject(new MockNativeObject(name, LunyPrimitiveType.Empty.ToString()));
		}

		public ILunyObject CreatePrimitive(String name, LunyPrimitiveType type)
		{
			Log.Add($"{nameof(CreatePrimitive)}({type},{name})");
			return new MockLunyObject(new MockNativeObject(name, type.ToString()));
		}
	}
}
