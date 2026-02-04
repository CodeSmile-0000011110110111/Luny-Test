using NUnit.Framework;
using System;

namespace Luny.Test.Core
{
	[TestFixture]
	public sealed class VariableTests
	{
		[Test]
		public void TestCreateAndProperties()
		{
			var v = Variable.Named(10, "MyVar");
			Assert.That(v.As<Double>(), Is.EqualTo(10.0));
#if DEBUG || LUNYSCRIPT_DEBUG
			Assert.That(v.Name, Is.EqualTo("MyVar"));
#else
			Assert.That(v.Name, Is.EqualTo("(N/A)"));
#endif
		}

		[Test]
		public void TestCreateDefaultName()
		{
			var v = (Variable)20;
			Assert.That(v.As<Double>(), Is.EqualTo(20.0));
			Assert.That(v.Name, Is.EqualTo("(N/A)"));
		}

		[Test]
		public void TestVariableCreate()
		{
			var v = Variable.Named(30, "IVar");
			Assert.That(v.As<Double>(), Is.EqualTo(30.0));
#if DEBUG || LUNYSCRIPT_DEBUG
			Assert.That(v.Name, Is.EqualTo("IVar"));
#else
			Assert.That(v.Name, Is.EqualTo("(N/A)"));
#endif
		}

		[Test]
		public void TestAsConversionMethods()
		{
			var vBool = (Variable)true;
			Assert.That(vBool.AsBoolean(), Is.True);

			var vNum = (Variable)123.45;
			Assert.That((Double)vNum.AsNumber(), Is.EqualTo(123.45));

			var vString = (Variable)"Hello";
			Assert.That(vString.AsString(), Is.EqualTo("Hello"));
		}

		[Test]
		public void TestImplicitConversionsToVariable()
		{
			Variable v;
			v = 10;
			Assert.That(v.As<Int64>(), Is.EqualTo(10));
			Assert.That(v.As<Int32>(), Is.EqualTo(10));
			v = 10.5f;
			Assert.That(v.As<Single>(), Is.EqualTo(10.5f));
			v = 10.5d;
			Assert.That(v.As<Double>(), Is.EqualTo(10.5));
			v = true;
			Assert.That(v.As<Boolean>(), Is.EqualTo(true));
			v = "Test";
			Assert.That(v.As<String>(), Is.EqualTo("Test"));
		}

		[Test]
		public void TestImplicitConversionsFromVariable()
		{
			Variable v;
			v = 10;
			Assert.That((Int64)v, Is.EqualTo(10));
			Assert.That((Int32)v, Is.EqualTo(10));
			v = 10.5f;
			Assert.That((Single)v, Is.EqualTo(10.5f));
			v = 10.5d;
			Assert.That((Double)v, Is.EqualTo(10.5));
			v = true;
			Assert.That((Boolean)v, Is.EqualTo(true));
			v = "Test";
			Assert.That((String)v, Is.EqualTo("Test"));
		}

		[Test]
		public void TestEquality()
		{
			var v1 = Variable.Named(10, "Var1");
			var v2 = Variable.Named(10, "Var2");
			var v3 = Variable.Named(20, "Var1");

			Assert.That(v1 == v2, Is.True);
			Assert.That(v1.Equals(v2), Is.True);
			Assert.That(v1 == v3, Is.False);

			Assert.That(v1 == 10, Is.True);
			Assert.That(v1 == 20, Is.False);
			Assert.That(10 == v1, Is.True);
		}

		[Test]
		public void TestFromOverloads()
		{
			Assert.That(((Variable)true).Type, Is.EqualTo(Variable.ValueType.Boolean));
			Assert.That(((Variable)10.0).Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That(((Variable)10.0f).Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That(((Variable)10).Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That(((Variable)10L).Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That(((Variable)(Number)10).Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That(((Variable)"Test").Type, Is.EqualTo(Variable.ValueType.String));
			Assert.That(new Variable().Type, Is.EqualTo(Variable.ValueType.Null));

			Assert.That(Variable.Named(true, "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named(10.0, "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named(10.0f, "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named(10, "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named(10L, "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named((Number)10, "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named("Test", "n").Name, Is.AnyOf("n", "(N/A)"));
			Assert.That(Variable.Named((Object)"n", "n").Name, Is.AnyOf("n", "(N/A)"));
		}

		[Test]
		public void TestIsTrueIsHighIsNormalized()
		{
			Variable vTrue = true;
			Variable vFalse = false;
			Variable v1 = 1.0;
			Variable v0 = 0.0;
			Variable vEps = 0.000000001;

			Assert.That(vTrue.IsTrue, Is.True);
			Assert.That(vFalse.IsTrue, Is.False);
			Assert.That(v1.IsTrue, Is.True);
			Assert.That(v0.IsTrue, Is.False);
			Assert.That(vEps.IsTrue, Is.True);

			Variable v04 = 0.4;
			Variable v05 = 0.5;
			Variable v06 = 0.6;
			Variable vM05 = -0.5;

			Assert.That(v04.IsHigh, Is.False);
			Assert.That(v05.IsHigh, Is.True);
			Assert.That(v06.IsHigh, Is.True);
			Assert.That(vM05.IsHigh, Is.True);

			Variable v10 = 1.0;
			Variable v11 = 1.1;
			Variable vM10 = -1.0;
			Variable vM11 = -1.1;

			Assert.That(v10.IsNormalized, Is.True);
			Assert.That(v11.IsNormalized, Is.False);
			Assert.That(vM10.IsNormalized, Is.True);
			Assert.That(vM11.IsNormalized, Is.False);
		}

		[Test]
		public void TestLength()
		{
			Variable vABC = "ABC";
			Variable vEmpty = "";
			Variable v10 = 10;
			var vNull = new Variable();

			Assert.That(vABC.Length, Is.EqualTo(3));
			Assert.That(vEmpty.Length, Is.EqualTo(0));
			Assert.That(v10.Length, Is.EqualTo(0));
			Assert.That(vNull.Length, Is.EqualTo(0));
		}

		[Test]
		public void TestEqualsExtended()
		{
			var vNum = (Variable)10.0;
			var vBool = (Variable)true;
			var vStr = (Variable)"Test";

			Assert.That(vNum.Equals(10.0), Is.True);
			Assert.That(vNum.Equals(5.0), Is.False);
			Assert.That(vBool.Equals(true), Is.True);
			Assert.That(vBool.Equals(false), Is.False);
			Assert.That(vStr.Equals("Test"), Is.True);
			Assert.That(vStr.Equals("Other"), Is.False);

			// Object Equals
			Assert.That(vNum.Equals((Object)10.0), Is.True);
			Assert.That(vBool.Equals((Object)true), Is.True);
			Assert.That(vStr.Equals((Object)"Test"), Is.True);
			Assert.That(vNum.Equals((Object)"Test"), Is.False);
			Assert.That(vNum.Equals(new Object()), Is.False);
		}

		[Test]
		public void TestNumberImplicitConversions()
		{
			Number n = 123.45;
			Variable v = n;
			Assert.That(v.Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That((Double)v, Is.EqualTo(123.45));

			var n2 = (Number)v;
			Assert.That((Double)n2, Is.EqualTo(123.45));
		}

		[Test]
		public void TestGetHashCode()
		{
			var v1 = (Variable)10;
			var v2 = (Variable)10;
			var v3 = (Variable)20;

			Assert.That(v1.GetHashCode(), Is.EqualTo(v2.GetHashCode()));
			Assert.That(v1.GetHashCode(), Is.Not.EqualTo(v3.GetHashCode()));
		}

		[Test]
		public void TestInequalityOperator()
		{
			var v1 = (Variable)10;
			var v2 = (Variable)10;
			var v3 = (Variable)20;

			Assert.That(v1 != v3, Is.True);
			Assert.That(v1 != v2, Is.False);
		}

		[Test]
		public void TestImplicitLongConversion()
		{
			var val = 1234567890123L;
			var v = (Variable)val;
			Assert.That(v.Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That((Int64)v, Is.EqualTo(val));
		}

		[Test]
		public void TestTryReadCoverage()
		{
			// ValueType.Number
			var vNum = (Variable)123.45;
			Assert.That(vNum.As<Single>(), Is.EqualTo(123.45f));
			Assert.That(vNum.As<Int32>(), Is.EqualTo(123));
			Assert.That(vNum.As<Int64>(), Is.EqualTo(123L));
			Assert.That(vNum.As<Double>(), Is.EqualTo(123.45));
			Assert.That(vNum.As<Number>(), Is.EqualTo((Number)123.45));
			Assert.That(vNum.As<Object>(), Is.EqualTo(123.45));

			// ValueType.Boolean
			var vBool = (Variable)true;
			Assert.That(vBool.As<Boolean>(), Is.True);
			Assert.That(vBool.As<Object>(), Is.EqualTo(true));

			// ValueType.String
			var vStr = (Variable)"Hello";
			Assert.That(vStr.As<String>(), Is.EqualTo("Hello"));
			Assert.That(vStr.As<Object>(), Is.EqualTo("Hello"));

			// ValueType.Null
			var vNull = new Variable();
			Assert.That(vNull.As<Object>(), Is.Null);

			// Unsupported conversions to trigger 'return false' in TryRead
			Assert.Throws<NotSupportedException>(() => vNum.As<String>());
			Assert.Throws<NotSupportedException>(() => vBool.As<Double>());
			Assert.Throws<NotSupportedException>(() => vStr.As<Double>());
			Assert.Throws<NotSupportedException>(() => vNull.As<Double>());
		}

		[Test]
		public void TestEqualsLunyVariableVariant()
		{
			var v1 = (Variable)10;
			var v2 = (Variable)10;
			var v3 = (Variable)"10";

			// This should call Equals(LunyVariable)
			Assert.That(v1.Equals(v2), Is.True);
			Assert.That(v1.Equals(v3), Is.False);
		}

		[Test]
		public void TestAsMethodsCoverage()
		{
			var vNum = (Variable)123.45;
			Assert.That(vNum.AsSingle(), Is.EqualTo(123.45f));
			Assert.That(vNum.AsInt64(), Is.EqualTo(123L));
			Assert.That(vNum.AsInt32(), Is.EqualTo(123));

			var vNull = new Variable();
			Assert.That(vNull.AsSingle(), Is.EqualTo(0f));
			Assert.That(vNull.AsInt64(), Is.EqualTo(0L));
			Assert.That(vNull.AsInt32(), Is.EqualTo(0));
			Assert.That(vNull.AsString(), Is.EqualTo(null));
			Assert.That((Double)vNull.AsNumber(), Is.EqualTo(0.0));
			Assert.That(vNull.AsDouble(), Is.EqualTo(0.0));
			Assert.That(vNull.AsBoolean(), Is.False);

			var vBool = (Variable)true;
			Assert.That(vBool.AsSingle(), Is.EqualTo(0f));
			Assert.That(vBool.AsInt64(), Is.EqualTo(0L));
			Assert.That(vBool.AsInt32(), Is.EqualTo(0));
		}

		[Test]
		public void TestToStringCoverage()
		{
			Variable v10 = 10;
			Variable vTrue = true;
			Variable vHello = "Hello";
			var vNull = new Variable();

			Assert.That(v10.ToString(), Contains.Substring("10"));
			Assert.That(vTrue.ToString(), Contains.Substring("True"));
			Assert.That(vHello.ToString(), Contains.Substring("Hello"));
			Assert.That(vNull.ToString(), Contains.Substring("Null"));
		}

		[Test]
		public void TestVariableEquals()
		{
			var vNum = (Variable)10.0;
			var vBool = (Variable)true;
			var vStr = (Variable)"Test";
			var vNull = new Variable();

			// Equals(Object) with other numeric types - should now pass
			Assert.That(vNum.Equals((Object)10), Is.True);
			Assert.That(vNum.Equals((Object)10L), Is.True);
			Assert.That(vNum.Equals((Object)10.0f), Is.True);
			Assert.That(vNum.Equals((Object)10U), Is.True);
			Assert.That(vNum.Equals((Object)10UL), Is.True);

			// Equals(String)
			Assert.That(vStr.Equals((String)null), Is.False);
			Assert.That(vNull.Equals((String)null), Is.False); // _type is Null, not String

			var vStrNull = (Variable)(String)null;
			Assert.That(vStrNull.Equals((String)null), Is.True);

			// Equals(Variable) with different types
			Assert.That(vNum.Equals(vBool), Is.False);
			Assert.That(vNum.Equals(vStr), Is.False);
			Assert.That(vNum.Equals(vNull), Is.False);
		}
	}
}
