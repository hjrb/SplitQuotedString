using BenchmarkDotNet.Running;
using ConsoleApp1;

var summary = BenchmarkRunner.Run<Benchmarks>();
