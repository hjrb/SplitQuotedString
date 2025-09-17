using hr.Tools;

namespace TestProject1;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethodEmpty()
    {
        Assert.IsTrue(StringSplit.GetTokens(string.Empty).ToArray().Length == 0);
        Assert.IsTrue(StringSplit.GetTokens(string.Empty.AsSpan(), false, StringSplit.DefaultQuoteChars.AsSpan(), StringSplit.DefaultQuoteChars.AsSpan()).ToArray().Length == 0);
        Assert.IsFalse(StringSplit.GetTokens(string.Empty.AsSpan()).Any());
    }

    [TestMethod]
    public void TestMethodDefault()
    {
        Assert.IsTrue(Enumerable.SequenceEqual(["a", "b", "c"], StringSplit.GetTokens("a b c").ToArray()));
        Assert.IsTrue(Enumerable.SequenceEqual(["a", "b", "c"], StringSplit.GetTokens("a b c").ToArray()));
    }
    [TestMethod]
    public void TestMethodQuoted()
    {
        Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c"], StringSplit.GetTokens("'a b' c").ToArray()));
        Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c"], StringSplit.GetTokens("\"a b\" c").ToArray()));
    }

    [TestMethod]
    public void TestMethodQuotedTab()
    {
        Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c", "d"], StringSplit.GetTokens("'a b'\tc d").ToArray()));
        Assert.IsTrue(Enumerable.SequenceEqual(["a b", "c", "d"], StringSplit.GetTokens("\"a b\"\tc d").ToArray()));
    }

    [TestMethod]
    public void TestMethodQuotedTabKeepEmpty()
    {
        var x = "a b  c".Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = StringSplit.GetTokens("a  b", treatConsequticveDelimitersAsOne: false).ToArray();
        Assert.IsTrue(Enumerable.SequenceEqual(["a", "", "b"], result));
        result = StringSplit.GetTokens("a  b", treatConsequticveDelimitersAsOne: true).ToArray();
        Assert.IsTrue(Enumerable.SequenceEqual(["a", "b"], result));
    }
}
