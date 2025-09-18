using HunnyR.Tools;

using nietras.SeparatedValues;

namespace TestProject1;

[TestClass]
public sealed class Test1
{
	 [TestMethod]
	 public void TestMethodEmpty()
	 {
		  Assert.IsTrue(new QuotedStringSplitter().Split(string.Empty).ToArray().Length == 0);
	 }

	 [TestMethod]
	 public void TestMethodSingleDelimiter()
	 {
		  var result = new QuotedStringSplitter() { Delimiters = [';'] }.Split(";").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.IsTrue(result[0].Length == 0);
		  Assert.IsTrue(result[1].Length == 0);
	}
	[TestMethod]
	public void TestMethodUseSep()
	{
	  foreach (var row in Sep.New(';').Reader(o=>o with {HasHeader=false}).FromText(";")) { 
			Assert.IsTrue(row.ColCount == 2);
		Assert.AreEqual(0, row[0].Span.Length);
		Assert.AreEqual(0, row[1].Span.Length);
		}
	  foreach (var row in Sep.New(';').Reader(o=>o with {HasHeader=false, Unescape=true }).FromText("\";\"")) { 
			Assert.IsTrue(row.ColCount == 1);
			var s=row[0].ToString();
			Assert.AreEqual(";", s);
		}

	}


	 [TestMethod]
	 public void TestMethodQuotes()
	 {
		  var result = new QuotedStringSplitter() { Delimiters = [';'] }.Split("'a;b';x").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.AreEqual("a;b", result[0]);
		  Assert.AreEqual("x", result[1]);
		  result = new QuotedStringSplitter() { Delimiters = [';'], }.Split("'\"a;b\"';x").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.AreEqual("\"a;b\"", result[0]);
		  Assert.AreEqual("x", result[1]);
		  result = new QuotedStringSplitter() { Delimiters = [';'], }.Split("'\"a;b';x").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.AreEqual("\"a;b", result[0]);
		  Assert.AreEqual("x", result[1]);
		  result = new QuotedStringSplitter() { Delimiters = [';'], }.Split("'\"a;b\";x").ToArray();
		  Assert.IsTrue(result.Length == 1);
		  Assert.AreEqual("\"a;b\";x", result[0]);
		  result = new QuotedStringSplitter() { Delimiters = [';'], }.Split("''a;b''").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.AreEqual("a", result[0]);
		  Assert.AreEqual("b", result[1]);
		  result = new QuotedStringSplitter() { Delimiters = [';'], }.Split("''a.b''").ToArray();
		  Assert.IsTrue(result.Length == 1);
		  Assert.AreEqual("a.b", result[0]);
		  result = new QuotedStringSplitter() { Delimiters = [';'], TreatTwoQuotesAsLiteral = true }.Split("''a.b''").ToArray();
		  Assert.IsTrue(result.Length == 1);
		  Assert.AreEqual("'a.b'", result[0]);
		  result = new QuotedStringSplitter() { Delimiters = ['.'], TreatTwoQuotesAsLiteral = true }.Split("''a.b''").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.AreEqual("'a", result[0]);
		  Assert.AreEqual("b'", result[1]);
	 }


	 [TestMethod]
	 public void TestMethodTwoDelimiter()
	 {
		  var result = new QuotedStringSplitter() { Delimiters = [',', ';'] }.Split(",;").ToArray();
		  Assert.IsTrue(result.Length == 3);
		  Assert.IsTrue(result[0].Length == 0);
		  Assert.IsTrue(result[1].Length == 0);
		  Assert.IsTrue(result[2].Length == 0);
		  result = new QuotedStringSplitter() { Delimiters = [',', '|'], TreatConsecutiveDelimitersAsOne = true }.Split(",|").ToArray();
		  Assert.IsTrue(result.Length == 2);
		  Assert.IsTrue(result[0].Length == 0);
		  Assert.IsTrue(result[1].Length == 0);
	 }

	 [TestMethod]
	 public void TestMethodDefault()
	 {
		  Assert.IsTrue(Enumerable.SequenceEqual(["a", "b", "c"], new QuotedStringSplitter().Split("a b c").ToArray()));
		  Assert.IsTrue(Enumerable.SequenceEqual(["a", "b", "c"], new QuotedStringSplitter().Split("a b c").ToArray()));
	 }

	 [TestMethod]
	 public void TestMethodQuoted()
	 {
		  Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c"], new QuotedStringSplitter().Split("'a b' c").ToArray()));
		  Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c"], new QuotedStringSplitter().Split("\"a b\" c").ToArray()));
	 }

	 [TestMethod]
	 public void TestMethodQuotedTab()
	 {
		  Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c", "d"], new QuotedStringSplitter().Split("'a b'\tc d").ToArray()));
		  Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c", "d"], new QuotedStringSplitter().Split("\"a b\"\tc d").ToArray()));
	 }

	 [TestMethod]
	 public void TestMethodQuotedTabKeepEmpty()
	 {
		  var result = new QuotedStringSplitter() { TreatConsecutiveDelimitersAsOne = false }.Split("a  b").ToArray();
		  Assert.IsTrue(Enumerable.SequenceEqual(["a", "", "b"], result));
		  result = new QuotedStringSplitter() { TreatConsecutiveDelimitersAsOne = true }.Split("a  b").ToArray();
		  Assert.IsTrue(Enumerable.SequenceEqual(["a", "b"], result));
	 }
}
