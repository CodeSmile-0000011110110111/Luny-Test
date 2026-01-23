using Luny.Engine.Services;
using System;

namespace Luny.Test
{
	public sealed class MockApplicationService : LunyApplicationServiceBase, ILunyApplicationService
	{
		public Boolean IsEditor => false;
		public Boolean IsPlaying => true;
		public void Quit(Int32 exitCode = 0) {}
	}
}
