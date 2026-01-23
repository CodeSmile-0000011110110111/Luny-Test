using Luny.Engine.Services;
using System;

namespace Luny.Test
{
	public sealed class MockTimeService : LunyTimeServiceBase, ILunyTimeService
	{
		public Int64 EngineFrameCount => 0;
		public Double ElapsedSeconds => 0;
	}
}
