using NUnit.Framework;
using System;

namespace Luny.Test
{
	[TestFixture]
	public class LunyVariableTests
	{
		[Test]
		public void TestCreateAndProperties()
		{
			var v = LunyVariable.Create("MyVar", 10);
			Assert.That(v.Value, Is.EqualTo(10));
#if DEBUG || LUNYSCRIPT_DEBUG
			Assert.That(v.Name, Is.EqualTo("MyVar"));
#else
			Assert.That(v.Name, Is.EqualTo("(N/A)"));
#endif
		}

		[Test]
		public void TestCreateDefaultName()
		{
			var v = LunyVariable.Create(20);
			Assert.That(v.Value, Is.EqualTo(20));
			Assert.That(v.Name, Is.EqualTo("(N/A)"));
		}

		[Test]
		public void TestIVariableCreate()
		{
			var v = ILunyVariable.Create("IVar", 30);
			Assert.That(v.Value, Is.EqualTo(30));
#if DEBUG || LUNYSCRIPT_DEBUG
			Assert.That(v.Name, Is.EqualTo("IVar"));
#else
			Assert.That(v.Name, Is.EqualTo("(N/A)"));
#endif
		}

		[Test]
		public void TestAsConversionMethods()
		{
			var vBool = LunyVariable.Create(true);
			Assert.That(vBool.AsBoolean(), Is.True);

			var vNum = LunyVariable.Create(123.45);
			Assert.That((Double)vNum.AsNumber(), Is.EqualTo(123.45));

			var vString = LunyVariable.Create("Hello");
			Assert.That(vString.AsString(), Is.EqualTo("Hello"));
		}

		[Test]
		public void TestImplicitConversions()
		{
			LunyVariable v;
			v = 10;
			Assert.That(v.Value, Is.EqualTo(10));
			v = 10.5f;
			Assert.That(v.Value, Is.EqualTo(10.5f));
			v = 10.5d;
			Assert.That(v.Value, Is.EqualTo(10.5d));
			v = true;
			Assert.That(v.Value, Is.EqualTo(true));
			v = "Test";
			Assert.That(v.Value, Is.EqualTo("Test"));
		}

		[Test]
		public void TestEquality()
		{
			var v1 = LunyVariable.Create("Var1", 10);
			var v2 = LunyVariable.Create("Var2", 10);
			var v3 = LunyVariable.Create("Var1", 20);

			Assert.That(v1 == v2, Is.True);
			Assert.That(v1.Equals(v2), Is.True);
			Assert.That(v1 == v3, Is.False);
			
			Assert.That(v1 == 10, Is.True);
			Assert.That(v1 == 20, Is.False);
			Assert.That(10 == v1, Is.True);
		}
	}
}
