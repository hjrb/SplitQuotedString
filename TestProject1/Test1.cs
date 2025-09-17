using HunnyR.Tools;

namespace TestProject1;

[TestClass]
public sealed class Test1
{
	[TestMethod]
	public void TestMethodEmpty()
	{
		Assert.IsTrue(new StringSplit().GetTokens(string.Empty).ToArray().Length == 0);
	}
	
	[TestMethod]
	public void TestMethodSingleDelimiter()
	{
		var result=new StringSplit() { Delimiters=[';']}.GetTokens(";").ToArray();
		Assert.IsTrue(result.Length == 2);
		Assert.IsTrue(result[0].Length==0);
		Assert.IsTrue(result[1].Length==0);
	}
	
	[TestMethod]
	public void TestMethodTwoDelimiter()
	{
		var result=new StringSplit() { Delimiters=[',',';']}.GetTokens(",;").ToArray();
		Assert.IsTrue(result.Length == 3);
		Assert.IsTrue(result[0].Length==0);
		Assert.IsTrue(result[1].Length==0);
		Assert.IsTrue(result[2].Length==0);
		result=new StringSplit() { Delimiters=[',','|'], TreatConsequticveDelimitersAsOne=true}.GetTokens(",|").ToArray();
		Assert.IsTrue(result.Length == 2);
		Assert.IsTrue(result[0].Length==0);
		Assert.IsTrue(result[1].Length==0);
	}

	[TestMethod]
	public void TestMethodDefault()
	{
		Assert.IsTrue(Enumerable.SequenceEqual(["a", "b", "c"], new StringSplit().GetTokens("a b c").ToArray()));
		Assert.IsTrue(Enumerable.SequenceEqual(["a", "b", "c"], new StringSplit().GetTokens("a b c").ToArray()));
	}

	[TestMethod]
	public void TestMethodQuoted()
	{
		Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c"], new StringSplit().GetTokens("'a b' c").ToArray()));
		Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c"], new StringSplit().GetTokens("\"a b\" c").ToArray()));
	}

	[TestMethod]
	public void TestMethodQuotedTab()
	{
		Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c", "d"], new StringSplit().GetTokens("'a b'\tc d").ToArray()));
		Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c", "d"], new StringSplit().GetTokens("\"a b\"\tc d").ToArray()));
	}

	[TestMethod]
	public void TestMethodQuotedTabKeepEmpty()
	{
		var result = new StringSplit() { TreatConsequticveDelimitersAsOne=false}.GetTokens("a  b").ToArray();
		Assert.IsTrue(Enumerable.SequenceEqual(["a", "", "b"], result));
		result = new StringSplit(){ TreatConsequticveDelimitersAsOne=true}.GetTokens("a  b").ToArray();
		Assert.IsTrue(Enumerable.SequenceEqual(["a", "b"], result));
	}
}
