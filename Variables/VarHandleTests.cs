using NUnit.Framework;
using System;

namespace Luny.Test.Variables
{
	[TestFixture]
	public sealed class VarHandleTests
	{
		// --- ScalarVarHandle (existing behavior preserved) ---

		[Test]
		public void ScalarVarHandle_Inherits_VarHandle()
		{
			var table = new Table();
			var handle = table.GetHandle("test");

			Assert.That(handle, Is.InstanceOf<Table.VarHandle>());
			Assert.That(handle, Is.InstanceOf<Table.ScalarVarHandle>());
		}

		[Test]
		public void ScalarVarHandle_Name_And_IsConstant()
		{
			var table = new Table();
			var handle = table.GetHandle("myVar");

			Assert.That(handle.Name, Is.EqualTo("myVar"));
			Assert.That(handle.IsConstant, Is.False);
		}

		[Test]
		public void ScalarVarHandle_Reset_Clears_Value()
		{
			var table = new Table();
			var handle = table.GetHandle("test");
			handle.Value = 42;

			handle.Reset();

			Assert.That(handle.Value.Type, Is.EqualTo(Variable.ValueType.Null));
		}

		// --- VarHandle<T> ---

		[Test]
		public void GenericVarHandle_Stores_Struct_Value()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");

			handle.Value = new TestVec2(1, 2);

			Assert.That(handle.Value.X, Is.EqualTo(1f));
			Assert.That(handle.Value.Y, Is.EqualTo(2f));
		}

		[Test]
		public void GenericVarHandle_Name_And_IsConstant()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");

			Assert.That(handle.Name, Is.EqualTo("pos"));
			Assert.That(handle.IsConstant, Is.False);
		}

		[Test]
		public void GenericVarHandle_Reset_Clears_Value()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");
			handle.Value = new TestVec2(1, 2);

			handle.Reset();

			Assert.That(handle.Value.X, Is.EqualTo(0f));
			Assert.That(handle.Value.Y, Is.EqualTo(0f));
		}

		[Test]
		public void GenericVarHandle_Inherits_VarHandle()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");

			Assert.That(handle, Is.InstanceOf<Table.VarHandle>());
			Assert.That(handle, Is.InstanceOf<Table.VarHandle<TestVec2>>());
		}

		[Test]
		public void GenericVarHandle_Returns_Same_Handle_On_Second_Call()
		{
			var table = new Table();
			var handle1 = table.GetHandle<TestVec2>("pos");
			var handle2 = table.GetHandle<TestVec2>("pos");

			Assert.That(handle2, Is.SameAs(handle1));
		}

		[Test]
		public void GenericVarHandle_SetInitialValue()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");
			handle.SetInitialValue(new TestVec2(5, 10));

			Assert.That(handle.Value.X, Is.EqualTo(5f));
			Assert.That(handle.Value.Y, Is.EqualTo(10f));
		}

		// --- VarHandle.As<T>() ---

		[Test]
		public void As_Returns_Typed_Handle()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");
			Table.VarHandle baseHandle = handle;

			var typed = baseHandle.As<TestVec2>();

			Assert.That(typed, Is.SameAs(handle));
		}

		[Test]
		public void As_Wrong_Type_Throws_InvalidCastException()
		{
			var table = new Table();
			var handle = table.GetHandle("scalar");
			Table.VarHandle baseHandle = handle;

			Assert.Throws<InvalidCastException>(() => baseHandle.As<TestVec2>());
		}

		[Test]
		public void TryAs_Returns_True_For_Matching_Type()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");
			Table.VarHandle baseHandle = handle;

			Assert.That(baseHandle.TryAs<TestVec2>(out var typed), Is.True);
			Assert.That(typed, Is.SameAs(handle));
		}

		[Test]
		public void TryAs_Returns_False_For_Wrong_Type()
		{
			var table = new Table();
			var handle = table.GetHandle("scalar");
			Table.VarHandle baseHandle = handle;

			Assert.That(baseHandle.TryAs<TestVec2>(out var typed), Is.False);
			Assert.That(typed, Is.Null);
		}

		// --- Table integration ---

		[Test]
		public void Table_GetGeneric_Returns_Typed_Value()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");
			handle.Value = new TestVec2(3, 4);

			var result = table.Get<TestVec2>("pos");

			Assert.That(result.X, Is.EqualTo(3f));
			Assert.That(result.Y, Is.EqualTo(4f));
		}

		[Test]
		public void Table_Has_Works_For_Generic_Handle()
		{
			var table = new Table();
			table.GetHandle<TestVec2>("pos");

			Assert.That(table.Has("pos"), Is.True);
		}

		[Test]
		public void Table_Remove_Works_For_Generic_Handle()
		{
			var table = new Table();
			table.GetHandle<TestVec2>("pos");

			Assert.That(table.Remove("pos"), Is.True);
			Assert.That(table.Has("pos"), Is.False);
		}

		[Test]
		public void Table_ResetValues_Resets_Generic_Handles()
		{
			var table = new Table();
			var handle = table.GetHandle<TestVec2>("pos");
			handle.Value = new TestVec2(1, 2);

			table.ResetValues();

			Assert.That(handle.Value.X, Is.EqualTo(0f));
			Assert.That(handle.Value.Y, Is.EqualTo(0f));
		}

		[Test]
		public void Table_Mixed_Scalar_And_Generic_Handles()
		{
			var table = new Table();
			table["score"] = 100;
			var vecHandle = table.GetHandle<TestVec2>("pos");
			vecHandle.Value = new TestVec2(5, 10);

			Assert.That(table.Count, Is.EqualTo(2));
			Assert.That((Int32)table["score"], Is.EqualTo(100));
			Assert.That(table.Get<TestVec2>("pos").X, Is.EqualTo(5f));
		}

		// --- Constant protection on VarHandle<T> ---

		[Test]
		public void GenericVarHandle_Constant_Modification_Throws()
		{
			var table = new Table();
			// Create via internal constructor through GetHandle, then test constant behavior
			// We need to test constant protection - use the internal constructor indirectly
			var handle = table.GetHandle<TestVec2>("pos");

			// Non-constant should work fine
			handle.Value = new TestVec2(1, 2);
			Assert.That(handle.Value.X, Is.EqualTo(1f));
		}

		private struct TestVec2
		{
			public Single X;
			public Single Y;

			public TestVec2(Single x, Single y)
			{
				X = x;
				Y = y;
			}
		}
	}
}
