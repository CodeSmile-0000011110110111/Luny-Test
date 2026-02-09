using NUnit.Framework;
using System;
using System.Linq;

namespace Luny.Test.Variables
{
	[TestFixture]
	public sealed class TableTests
	{
		[Test]
		public void TestIndexerGetAndSet()
		{
			var table = new Table();
			table["key1"] = 123;
			table["key2"] = "value2";

			Assert.That(table["key1"].Type, Is.EqualTo(Variable.ValueType.Number));
			Assert.That((Int32)table["key1"], Is.EqualTo(123));
			Assert.That((String)table["key2"], Is.EqualTo("value2"));
			Assert.That(table["key3"].Type, Is.EqualTo(Variable.ValueType.String));
			Assert.That(table["key3"].AsString(), Is.EqualTo(String.Empty));
		}

		[Test]
		public void TestCount()
		{
			var table = new Table();
			Assert.That(table.Count, Is.EqualTo(0));

			table["key1"] = 1;
			Assert.That(table.Count, Is.EqualTo(1));

			table["key2"] = 2;
			Assert.That(table.Count, Is.EqualTo(2));

			table["key1"] = 3;
			Assert.That(table.Count, Is.EqualTo(2));

			table.Remove("key1");
			Assert.That(table.Count, Is.EqualTo(1));

			table.RemoveAll();
			Assert.That(table.Count, Is.EqualTo(0));
		}

		[Test]
		public void TestGetWithCast()
		{
			var table = new Table();
			table["num"] = 456;
			table["str"] = "hello";

			Assert.That(table.Get<Int32>("num"), Is.EqualTo(456));
			Assert.That(table.Get<String>("str"), Is.EqualTo("hello"));
			Assert.That(table.Get<Int32>("nonexistent"), Is.EqualTo(0));
			Assert.That(table.Get<String>("nonexistent"), Is.Null);
		}

		[Test]
		public void TestHas()
		{
			var table = new Table();
			table["key1"] = 1;

			Assert.That(table.Has("key1"), Is.True);
			Assert.That(table.Has("key2"), Is.False);
		}

		[Test]
		public void TestRemove()
		{
			var table = new Table();
			table["key1"] = 1;

			Assert.That(table.Remove("key1"), Is.True);
			Assert.That(table.Has("key1"), Is.False);
			Assert.That(table.Remove("key1"), Is.False);
		}

		[Test]
		public void TestClear()
		{
			var table = new Table();
			table["key1"] = 1;
			table["key2"] = 2;

			table.RemoveAll();

			Assert.That(table.Count, Is.EqualTo(0));
			Assert.That(table.Has("key1"), Is.False);
			Assert.That(table.Has("key2"), Is.False);
		}

		[Test]
		public void TestEnumerator()
		{
			var table = new Table();
			table["a"] = 1;
			table["b"] = 2;

			var list = table.ToList();
			Assert.That(list.Count, Is.EqualTo(2));

			var a = list.FirstOrDefault(kvp => kvp.Key == "a");
			Assert.That((Int32)a.Value, Is.EqualTo(1));

			var b = list.FirstOrDefault(kvp => kvp.Key == "b");
			Assert.That((Int32)b.Value, Is.EqualTo(2));
		}

		[Test]
		public void DefineConstant_Creates_ReadOnly_Handle()
		{
			var table = new Table();
			var handle = table.DefineConstant("PI", 3.14159);

			Assert.That(handle.IsConstant, Is.True);
			Assert.That(handle.Name, Is.EqualTo("PI"));
			Assert.That((Double)handle.Value, Is.EqualTo(3.14159).Within(0.00001));
		}

		[Test]
		public void DefineConstant_Modification_Throws()
		{
			var table = new Table();
			table.DefineConstant("PI", 3.14159);

			Assert.Throws<InvalidOperationException>(() => table["PI"] = 4);
		}

		[Test]
		public void DefineConstant_Handle_Modification_Throws()
		{
			var table = new Table();
			var handle = table.DefineConstant("PI", 3.14159);

			Assert.Throws<InvalidOperationException>(() => handle.Value = 4);
		}

		[Test]
		public void DefineConstant_Redefinition_Throws()
		{
			var table = new Table();
			table.DefineConstant("PI", 3.14159);

			Assert.Throws<InvalidOperationException>(() => table.DefineConstant("PI", 6.28318));
		}

		[Test]
		public void GetHandle_Returns_Non_Constant_Handle()
		{
			var table = new Table();
			var handle = table.GetHandle("mutable");

			Assert.That(handle.IsConstant, Is.False);
			handle.Value = 123;
			Assert.That((Int32)handle.Value, Is.EqualTo(123));
		}

		[Test]
		public void TestOnVariableChangedEvent()
		{
#if DEBUG || LUNYSCRIPT_DEBUG
			var table = new Table();
			String changedName = null;
			Variable changedCurrent = null;
			Variable changedPrevious = null;
			var callCount = 0;

			table.OnVariableChanged += (sender, args) =>
			{
				changedName = args.Name;
				changedCurrent = args.Current;
				changedPrevious = args.Previous;
				callCount++;
			};

			table["var1"] = 10;
			Assert.That(callCount, Is.EqualTo(1));
			Assert.That(changedName, Is.EqualTo("var1"));
			Assert.That((Int32)changedCurrent, Is.EqualTo(10));
			Assert.That(changedPrevious.Type, Is.EqualTo(Variable.ValueType.Null));
			Assert.That(changedPrevious.AsString(), Is.EqualTo(null));

			table["var1"] = 20;
			Assert.That(callCount, Is.EqualTo(2));
			Assert.That(changedName, Is.EqualTo("var1"));
			Assert.That((Int32)changedCurrent, Is.EqualTo(20));
			Assert.That((Int32)changedPrevious, Is.EqualTo(10));
#else
			Assert.Pass("OnVariableChanged is only active in DEBUG or LUNYSCRIPT_DEBUG");
#endif
		}
	}
}
