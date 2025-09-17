using BenchmarkDotNet.Attributes;
using HunnyR.Tools;
using System.Linq;
using Microsoft.VSDiagnostics;
using System.Collections.Generic;

namespace SplitQuotedString.Benchmarks
{
    [CPUUsageDiagnoser]
    public class StringSplitBenchmarks
    {
        private string testString = "one \"two three\" 'four five' six  seven";
        [Benchmark]
        public List<string> GetTokens_Default()
        {
            return new StringSplit().GetTokens(testString).ToList();
        }
    }
}