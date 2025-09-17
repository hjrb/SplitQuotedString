using BenchmarkDotNet.Attributes;
using HunnyR.Tools;
using Microsoft.VSDiagnostics;
using System.Collections.Generic;
using System.Linq;

namespace SplitQuotedString.Benchmarks
{
    [CPUUsageDiagnoser]
    public class RawStringSplitBenchmarks
    {
        private readonly string testString = "one \"two three\" 'four five' six  seven";
        private readonly QuotedStringSplitter splitter = new();

        [Benchmark(Baseline = true)]
        public List<string> GetTokens_Default()
        {
            return splitter.Split(testString).ToList();
        }
    }
}
