using NUnit.Framework;
using System;
using System.Globalization;

namespace Luny.Test.Variables
{
	[TestFixture]
	public sealed class NumberTests
	{
		[Test]
		public void TestToTimeSpan()
		{
			var ts = TimeSpan.FromSeconds(1);
			Number n = ts;
			Assert.That(n.ToTimeSpan(null), Is.EqualTo(ts));

			// Test clamping for ToTimeSpan
			Number large = 1e30;
			Assert.That(large.ToTimeSpan(null).Ticks, Is.EqualTo(Int64.MaxValue));
			Number small = -1e30;
			Assert.That(small.ToTimeSpan(null).Ticks, Is.EqualTo(Int64.MinValue));
		}

		[Test]
		public void TestArithmeticWithBool()
		{
			Number n = 5;
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n + true;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = true + n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n - true;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = true - n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n * true;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = true * n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n / true;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = true / n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n % true;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = true % n;
			});
		}

		[Test]
		public void TestArithmeticWithString()
		{
			Number n = 5;
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n + "10";
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = "10" + n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n - "10";
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = "10" - n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n * "10";
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = "10" * n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n / "10";
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = "10" / n;
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = n % "10";
			});
			Assert.Throws<InvalidOperationException>(() =>
			{
				var x = "10" % n;
			});
		}

		[Test]
		public void TestStringConversion()
		{
			Assert.That((Double)new Number("10"), Is.EqualTo(10.0));
			Assert.That((Double)new Number("abc"), Is.EqualTo(0.0));
			Assert.That((Double)new Number(""), Is.EqualTo(0.0));
			Assert.That((Double)new Number(null), Is.EqualTo(0.0));
		}

		[Test]
		public void TestBoolConversion()
		{
			Assert.That((Double)new Number(true), Is.EqualTo(1.0));
			Assert.That((Double)new Number(false), Is.EqualTo(0.0));
		}

		[Test]
		public void TestImplicitConversionsFrom()
		{
			Number n;
			n = 5.5d;
			Assert.That((Double)n, Is.EqualTo(5.5d));
			n = 5.5f;
			Assert.That((Single)n, Is.EqualTo(5.5f));
			n = 5;
			Assert.That((Int32)n, Is.EqualTo(5));
			n = (Int64)5;
			Assert.That((Int64)n, Is.EqualTo(5));
			n = (Int16)5;
			Assert.That((Int16)n, Is.EqualTo(5));
			n = (Byte)5;
			Assert.That((Byte)n, Is.EqualTo(5));
			n = (UInt32)5;
			Assert.That((UInt32)n, Is.EqualTo(5));
			n = (UInt64)5;
			Assert.That((UInt64)n, Is.EqualTo(5));
			n = (UInt16)5;
			Assert.That((UInt16)n, Is.EqualTo(5));
			n = (SByte)5;
			Assert.That((SByte)n, Is.EqualTo(5));
			n = 5.5m;
			Assert.That((Decimal)n, Is.EqualTo(5.5m));
			n = true;
			Assert.That((Boolean)n, Is.True);
			n = false;
			Assert.That((Boolean)n, Is.False);

			var ts = TimeSpan.FromSeconds(1);
			n = ts;
			Assert.That((TimeSpan)n, Is.EqualTo(ts));

			// DateTime conversion via double loses precision, but should be within a reasonable delta
			var dt = DateTime.Now;
			n = dt;
			Assert.That((DateTime)n, Is.EqualTo(dt).Within(TimeSpan.FromTicks(666))); // avoids test failure due to slight discrepancies

			n = "123.45";
			Assert.That((Double)n, Is.EqualTo(123.45d));
		}

		[Test]
		public void TestImplicitConversionsTo()
		{
			Number n = 123.45;
			Assert.That((Double)n, Is.EqualTo(123.45d));
			Assert.That((Single)n, Is.EqualTo(123.45f));
			Assert.That((Int32)n, Is.EqualTo(123));
			Assert.That((Int64)n, Is.EqualTo(123));
			Assert.That((Int16)n, Is.EqualTo(123));
			Assert.That((Byte)n, Is.EqualTo(123));
			Assert.That((UInt32)n, Is.EqualTo(123));
			Assert.That((UInt64)n, Is.EqualTo(123));
			Assert.That((UInt16)n, Is.EqualTo(123));
			Assert.That((SByte)n, Is.EqualTo(123));
			Assert.That((Decimal)n, Is.EqualTo(123.45m));
			Assert.That((String)n, Is.EqualTo("123.45"));

			n = 1.0;
			Assert.That((Boolean)n, Is.True);
			n = 0.0;
			Assert.That((Boolean)n, Is.False);
			n = 0.0000000000001; // Should be true if > Epsilon
			Assert.That((Boolean)n, Is.True);
		}

		[Test]
		public void TestArithmeticOperators()
		{
			Number a = 10;
			Number b = 5;
			Assert.That(a + b, Is.EqualTo((Number)15));
			Assert.That(a - b, Is.EqualTo((Number)5));
			Assert.That(a * b, Is.EqualTo((Number)50));
			Assert.That(a / b, Is.EqualTo((Number)2));
			Assert.That(a % b, Is.EqualTo((Number)0));

			Assert.That(+a, Is.EqualTo((Number)10));
			Assert.That(-a, Is.EqualTo((Number)(-10)));
		}

		[Test]
		public void TestComparisonOperators()
		{
			Number a = 10;
			Number b = 5;
			Number c = 10;

			Assert.That(a == c, Is.True);
			Assert.That(a != b, Is.True);
			Assert.That(a > b, Is.True);
			Assert.That(b < a, Is.True);
			Assert.That(a >= b, Is.True);
			Assert.That(a >= c, Is.True);
			Assert.That(b <= a, Is.True);
#pragma warning disable 1718 // comparison made to same variable
			Assert.That(b <= b, Is.True);
#pragma warning restore 1718
		}

		[Test]
		public void TestIConvertible()
		{
			IConvertible conv = (Number)123.45;
			Assert.That(conv.GetTypeCode(), Is.EqualTo(TypeCode.Double));
			Assert.That(conv.ToBoolean(null), Is.True);
			Assert.That(conv.ToByte(null), Is.EqualTo((Byte)123));
			Assert.That(conv.ToDouble(null), Is.EqualTo(123.45d));
			Assert.That(conv.ToInt32(null), Is.EqualTo(123));
			Assert.That(conv.ToSingle(null), Is.EqualTo(123.45f));
			Assert.That(conv.ToDecimal(null), Is.EqualTo(123.45m));
			Assert.That(conv.ToInt16(null), Is.EqualTo((Int16)123));
			Assert.That(conv.ToInt64(null), Is.EqualTo((Int64)123));
			Assert.That(conv.ToSByte(null), Is.EqualTo((SByte)123));
			Assert.That(conv.ToUInt16(null), Is.EqualTo((UInt16)123));
			Assert.That(conv.ToUInt32(null), Is.EqualTo((UInt32)123));
			Assert.That(conv.ToUInt64(null), Is.EqualTo((UInt64)123));
			Assert.That(conv.ToString(CultureInfo.InvariantCulture), Is.EqualTo("123.45"));
			Assert.That(conv.ToType(typeof(Double), null), Is.EqualTo(123.45d));

			Assert.That(conv.ToDateTime(null), Is.EqualTo(DateTime.FromBinary(123)));
			Assert.That(((Number)123).ToChar(CultureInfo.InvariantCulture), Is.EqualTo((Char)123));

			// Test clamping/safety for ToChar
			Assert.That(((Number)70000).ToChar(null), Is.EqualTo(Char.MaxValue));
			Assert.That(((Number)(-1)).ToChar(null), Is.EqualTo(Char.MinValue));
		}

		[Test]
		public void TestIComparable()
		{
			Number a = 10;
			Number b = 5;
			Number c = 10;

			Assert.That(a.CompareTo(b), Is.GreaterThan(0));
			Assert.That(b.CompareTo(a), Is.LessThan(0));
			Assert.That(a.CompareTo(c), Is.EqualTo(0));

			Assert.That(a.CompareTo((Object)5.0d), Is.GreaterThan(0));
			Assert.That(a.CompareTo((Object)10.0d), Is.EqualTo(0));
			Assert.That(a.CompareTo(null), Is.EqualTo(1));

			Assert.That(a.CompareTo(5.0d), Is.GreaterThan(0));
			Assert.That(a.CompareTo((Int64)5), Is.GreaterThan(0));
			Assert.That(a.CompareTo((UInt64)5), Is.GreaterThan(0));

			Assert.Throws<ArgumentException>(() => a.CompareTo(Guid.NewGuid()));
		}

		[Test]
		public void TestIEquatable()
		{
			Number a = 10;
			Number b = 5;
			Number c = 10;

			Assert.That(a.Equals(c), Is.True);
			Assert.That(a.Equals(b), Is.False);
			Assert.That(a.Equals((Object)10.0d), Is.True);
			Assert.That(a.Equals((Object)5.0d), Is.False);
			Assert.That(a.Equals(new Object()), Is.False);

			Assert.That(a.Equals(10.0d), Is.True);
			Assert.That(a.Equals((Int64)10), Is.True);
			Assert.That(a.Equals((UInt64)10), Is.True);
		}

		[Test]
		public void TestIFormattable()
		{
			Number n = 123.456;
			Assert.That(n.ToString("F2", CultureInfo.InvariantCulture), Is.EqualTo("123.46"));
		}

		[Test]
		public void TestGetHashCode()
		{
			Number a = 10;
			Number b = 10;
			Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
		}

		[Test]
		public void TestNumberEquals()
		{
			Number n = 10.0;
			// IEquatable<Int64>
			Assert.That(n.Equals(10L), Is.True);
			Assert.That(n.Equals(5L), Is.False);

			// IEquatable<UInt64>
			Assert.That(n.Equals(10UL), Is.True);
			Assert.That(n.Equals(5UL), Is.False);

			// Equals(Object) with Int64/UInt64
			Assert.That(n.Equals((Object)10L), Is.True);
			Assert.That(n.Equals((Object)10UL), Is.True);

			// Equals(Object) with Single/Int32/UInt32
			Assert.That(n.Equals((Object)10.0f), Is.True);
			Assert.That(n.Equals((Object)10), Is.True);
			Assert.That(n.Equals((Object)10U), Is.True);

			// Edge case: NaN
			Number nan = Double.NaN;
			Assert.That(nan.Equals(Double.NaN), Is.True);
			// In double comparisons, NaN != NaN. But double.Equals(double) is true for both NaN.
			// Number uses double.Equals internally for both Equals and operator ==.
			Assert.That((Double)nan == Double.NaN, Is.False);
			Assert.That(nan == Double.NaN, Is.True);
		}

		[Test]
		public void TestNumberCompareTo()
		{
			Number n = 10.0;

			// IComparable<Int64>
			Assert.That(n.CompareTo(10L), Is.EqualTo(0));
			Assert.That(n.CompareTo(5L), Is.GreaterThan(0));
			Assert.That(n.CompareTo(15L), Is.LessThan(0));

			// IComparable<UInt64>
			Assert.That(n.CompareTo(10UL), Is.EqualTo(0));
			Assert.That(n.CompareTo(5UL), Is.GreaterThan(0));
			Assert.That(n.CompareTo(15UL), Is.LessThan(0));
		}
	}
}
