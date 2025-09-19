using BenchmarkDotNet.Attributes;

using HunnyR.Tools;

using nietras.SeparatedValues;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class Benchmarks
{
	const string input = "one two 'three four' five \"six seven\" eight,,nine,,ten 'eleven twelve' \"thirteen fourteen\"";

	[Benchmark]
	public void SplitQuotedString()
	{
		var splitter=new QuotedStringSplitter();
		var list=splitter.Split(input).ToList();
	}

	[Benchmark]
	public void SplitUsingSep()
	{
		var list=new List<string>();
		foreach (var row in Sep.New(',').Reader(o=>o with {HasHeader=false}).FromText(input)) { 
			for (int i=0;i<row.ColCount;i++) {
				list.Add(row[i].ToString());
			}
		}
	 // foreach (var row in Sep.New(';').Reader(o=>o with {HasHeader=false, Unescape=true }).FromText("\";\"")) { 
		//	Assert.IsTrue(row.ColCount == 1);
		//	var s=row[0].ToString();
		//	Assert.AreEqual(";", s);
		//}

	}
}


