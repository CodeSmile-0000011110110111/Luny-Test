using System;
using System.Runtime.CompilerServices;

namespace Luny.Test
{
	public static class Helper
	{
		public static String GetCurrentMethodName([CallerMemberName] String name = "") => name;
	}
}
