using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark]
    public void SplitQuotedString()
    {
        var input = "one two 'three four' five \"six seven\" eight,,nine,,ten 'eleven twelve' \"thirteen fourteen\"";
        _ = HunnyR.Tools.QuotedStringSplitter.Split(input, new HashSet<char> { ' ', ',' }, new HashSet<char> { '\'', '"' }, treatConsecutiveDelimitersAsOne: true, treatTwoQuotesAsLiteral: false).ToList();
    }
}


