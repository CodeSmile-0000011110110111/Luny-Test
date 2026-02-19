using Luny.Engine.Bridge;
using NUnit.Framework;
using System;

namespace Luny.Test.Variables
{
	[TestFixture]
	public sealed class VariableGenericTests
	{
		private struct TestVector3
		{
			public Single X;
			public Single Y;
			public Single Z;

			public TestVector3(Single x, Single y, Single z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public override String ToString() => $"({X}, {Y}, {Z})";
		}

		[Test]
		public void Constructor_Stores_Value()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(v.Value.X, Is.EqualTo(1f));
			Assert.That(v.Value.Y, Is.EqualTo(2f));
			Assert.That(v.Value.Z, Is.EqualTo(3f));
		}

		[Test]
		public void Type_Returns_Struct()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(v.Type, Is.EqualTo(Variable.ValueType.Struct));
		}

		[Test]
		public void AsBoolean_Returns_True_For_NonNull()
		{
			var v = new Variable<TestVector3>(new TestVector3(0, 0, 0));

			Assert.That(v.AsBoolean(), Is.True);
		}

		[Test]
		public void AsDouble_Returns_Zero()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(v.AsDouble(), Is.EqualTo(0.0));
		}

		[Test]
		public void AsString_Returns_Value_ToString()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(v.AsString(), Is.EqualTo("(1, 2, 3)"));
		}

		[Test]
		public void Equals_Same_Value_Returns_True()
		{
			var a = new Variable<TestVector3>(new TestVector3(1, 2, 3));
			var b = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(a.Equals(b), Is.True);
			Assert.That(a == b, Is.True);
		}

		[Test]
		public void Equals_Different_Value_Returns_False()
		{
			var a = new Variable<TestVector3>(new TestVector3(1, 2, 3));
			var b = new Variable<TestVector3>(new TestVector3(4, 5, 6));

			Assert.That(a.Equals(b), Is.False);
			Assert.That(a != b, Is.True);
		}

		[Test]
		public void Equals_Object_Returns_Correct_Result()
		{
			var a = new Variable<TestVector3>(new TestVector3(1, 2, 3));
			var b = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(a.Equals((Object)b), Is.True);
			Assert.That(a.Equals("not a variable"), Is.False);
		}

		[Test]
		public void GetHashCode_Same_Value_Returns_Same_Hash()
		{
			var a = new Variable<TestVector3>(new TestVector3(1, 2, 3));
			var b = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
		}

		[Test]
		public void Implicit_Conversion_From_T()
		{
			Variable<TestVector3> v = new TestVector3(1, 2, 3);

			Assert.That(v.Value.X, Is.EqualTo(1f));
		}

		[Test]
		public void Implicit_Conversion_To_T()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));
			TestVector3 raw = v;

			Assert.That(raw.X, Is.EqualTo(1f));
			Assert.That(raw.Y, Is.EqualTo(2f));
			Assert.That(raw.Z, Is.EqualTo(3f));
		}

		[Test]
		public void IVariable_Interface_Works()
		{
			IVariable iv = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(iv.Type, Is.EqualTo(Variable.ValueType.Struct));
			Assert.That(iv.AsBoolean(), Is.True);
			Assert.That(iv.AsDouble(), Is.EqualTo(0.0));
			Assert.That(iv.AsString(), Is.EqualTo("(1, 2, 3)"));
		}

		[Test]
		public void ToString_Returns_Value_String()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(v.ToString(), Is.EqualTo("(1, 2, 3)"));
		}

		[Test]
		public void Works_With_Reference_Types()
		{
			var v = new Variable<String>("hello");

			Assert.That(v.Value, Is.EqualTo("hello"));
			Assert.That(v.AsString(), Is.EqualTo("hello"));
			Assert.That(v.AsBoolean(), Is.True);
		}

		[Test]
		public void Default_Struct_Has_Null_String_Representation()
		{
			var v = default(Variable<TestVector3>);

			Assert.That(v.AsString(), Is.EqualTo("(0, 0, 0)"));
		}

		[Test]
		public void LunyVector2_Type_Returns_Vector2()
		{
			var v = new Variable<LunyVector2>(new LunyVector2(3f, 4f));

			Assert.That(v.Type, Is.EqualTo(Variable.ValueType.Vector2));
		}

		[Test]
		public void LunyVector2_AsVector2_Returns_Value()
		{
			var vec = new LunyVector2(3f, 4f);
			var v = new Variable<LunyVector2>(vec);

			Assert.That(v.AsVector2(), Is.EqualTo(vec));
			Assert.That(v.AsVector3(), Is.EqualTo(default(LunyVector3)));
		}

		[Test]
		public void LunyVector3_Type_Returns_Vector3()
		{
			var v = new Variable<LunyVector3>(new LunyVector3(1f, 2f, 3f));

			Assert.That(v.Type, Is.EqualTo(Variable.ValueType.Vector3));
		}

		[Test]
		public void LunyVector3_AsVector3_Returns_Value()
		{
			var vec = new LunyVector3(1f, 2f, 3f);
			var v = new Variable<LunyVector3>(vec);

			Assert.That(v.AsVector3(), Is.EqualTo(vec));
			Assert.That(v.AsVector2(), Is.EqualTo(default(LunyVector2)));
		}

		[Test]
		public void NonVector_Struct_AsVector_Returns_Default()
		{
			var v = new Variable<TestVector3>(new TestVector3(1, 2, 3));

			Assert.That(v.AsVector2(), Is.EqualTo(default(LunyVector2)));
			Assert.That(v.AsVector3(), Is.EqualTo(default(LunyVector3)));
		}
	}
}
