using hr.Tools;

namespace TestProject1;

[TestClass]
public sealed class Test1
{
	[TestMethod]
	public void TestMethod1()
	{
		Assert.IsTrue(StringSplit.GetTokens(string.Empty).ToArray().Length==0);
	}
}
