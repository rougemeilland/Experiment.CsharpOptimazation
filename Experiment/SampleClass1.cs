using System;

namespace Experiment
{
    internal sealed class SampleClass1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:メンバーを static に設定します", Justification = "実験用メソッドであるのでstaticにはしない。")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:不要な抑制を削除します", Justification = "実験用メソッドであるため属性を削除しない。")]
        public void Execute(string s) => Console.WriteLine($"Called 'Execute(\"{s}\")'");
    }
}
