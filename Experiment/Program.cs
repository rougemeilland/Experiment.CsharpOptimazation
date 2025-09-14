using System;
using System.IO;
using System.Runtime.CompilerServices;
using Experiment.RootClassLibrary;

namespace Experiment
{
    internal sealed class Program
    {
        private static void Main()
        {
            //
            // 注意:
            //   Experiment.RootClassLibrary のビルドオプションで、デバッグシンボルは生成しないように指定する。
            //   デバッグシンボルが生成されると、デコンパイルによってILを調べるのが面倒になるため。
            //

            TestNullConditionOperator();
            TestOptimazationForMulAndDiv();
            TestAssertion();
            TestInlining();
            TestGenerics();
            Console.WriteLine();
            Console.WriteLine("----------");
            Console.WriteLine();
            Console.WriteLine("Completed.");
            Console.Beep();
            _ = Console.ReadLine();
        }

        // obj.?Foo(<expression>) のメソッド呼び出しにおいて、objが null の場合に <expression> は評価されるか?
        // => obj が null の場合は、<expression> は評価されない。
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestNullConditionOperator()
        {
            Console.WriteLine();
            Console.WriteLine($"----- {nameof(TestNullConditionOperator)} -----");
            Console.WriteLine() ;

            var obj1 = (SampleClass1?)new SampleClass1();

            Console.WriteLine("'obj?.Execute(GetStringParameter())' の呼び出し (obj は 非null");

            // obj1 が null ではない場合、 GetStringParameter() は呼び出される。
            obj1?.Execute(GetStringParameter());

            Console.WriteLine();

            Console.WriteLine("'obj?.Execute(GetStringParameter())' の呼び出し (obj は null");
            var obj2 = (SampleClass1?)null;
            // obj2 が null である場合、 GetStringParameter() は呼び出されない。
            obj2?.Execute(GetStringParameter());

            obj2?.Execute(GetStringParameter());

        }

        // 2 のべき乗での乗除算および剰余は シフトまたはマスク演算に最適化されるか?
        // => IL では最適化されない。JIT による x64 機械語へのアセンブル時に最適化される。
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestOptimazationForMulAndDiv()
        {
            Console.WriteLine(nameof(OptimizeMulAndDiv));
            //
            // 注意:
            // 最適化の効果を見たいので、"Experiment.RootClassLibrary" プロジェクトを依存関係に設定するのではなく
            // "Experiment.RootClassLibrary" から生成された Release 版のアセンブリを "参照" で追加すること。
            //  

            Console.WriteLine();
            Console.WriteLine($"----- {nameof(TestOptimazationForMulAndDiv)} -----");
            Console.WriteLine();

            var value = Environment.TickCount; // 定数ではない値であれば何でもいい

            // 符号付整数の乗算
            var mul = OptimizeMulAndDiv.MultiplyBy1024(value); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{value} * 1024 => {mul}");

            // 符号付整数の除算
            var div = OptimizeMulAndDiv.DivideBy1024(value); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{value} / 1024 => {div}");

            // 符号付整数の剰余
            var rem = OptimizeMulAndDiv.RemainderAt1024(value); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{value} % 1024 => {rem}");

            var unsignedValue = checked((uint)Environment.TickCount); // 定数ではない値であれば何でもいい

            // 符号無し整数の乗算
            var umul = OptimizeMulAndDiv.UnsignedMultiplyBy1024(unsignedValue); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{unsignedValue}U * 1024 => {umul}U");

            // 符号無し整数の除算
            var udiv = OptimizeMulAndDiv.UnsignedDivideBy1024(unsignedValue); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{unsignedValue}U / 1024 => {udiv}U");

            // 符号無し整数の剰余
            var urem = OptimizeMulAndDiv.UnsignedRemainderAt1024(unsignedValue); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{unsignedValue}U % 1024 => {urem}U");
        }

        // System.Diagnostics.Debug.Assert(<condition>) において、<condition> の計算が副作用を持つ場合、Release 版ではその副作用は実行されるか?
        // => 副作用は実行されない。例えば、
        //      System.Diagnostics.Debug.Assert(Foo() == value);
        //    のようなステートメントは、Release 版では
        //      _ = (Foo() == value);
        //    と同等になるのではなく、ただの空文となる。
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestAssertion()
        {
            //
            // 注意:
            // Release版での結果を見たいので、"Experiment.RootClassLibrary" プロジェクトを依存関係に設定するのではなく
            // "Experiment.RootClassLibrary" から生成された Release 版のアセンブリを "参照" で追加すること。
            //  

            Console.WriteLine();
            Console.WriteLine($"----- {nameof(TestAssertion)} -----");
            Console.WriteLine();

            var baseDirectoryPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? throw new Exception();
            var contentFilePath = Path.Combine(baseDirectoryPath, "content.txt");
            using var inStream = new FileStream(contentFilePath, FileMode.Open, FileAccess.Read);
            Span<byte> buffer = stackalloc byte[5];
            Assertion.ReadBuffer(inStream, buffer); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。

            // 'Assertion.ReadBuffer()' では実質何も行われないので、buffer の内容は常に初期値から変わっていない。(つまりすべて 0)
            Console.WriteLine($"buffer => [0x{buffer[0]:x2}, 0x{buffer[1]:x2}, 0x{buffer[2]:x2}, 0x{buffer[3]:x2}, 0x{buffer[4]:x2}]");
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)] 属性のある単純な public メソッドは、他のアセンブリにもインライン展開されるか?
        // => 展開される。
        //    ただし、既定では Quick JIT という機能が有効になっており、これが有効だと最適化よりもコンパイル速度 (≒アプリケーションの起動速度) が優先され、インライン展開そのものが行われない。
        //    対策は、アプリケーションプロジェクトのプロジェクトファイルに "<TieredCompilationQuickJit>false</TieredCompilationQuickJit>" を追加する。クラスライブラリのプロジェクトファイルには追加不要。
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestInlining()
        {
            //
            // 注意:
            // Release版での結果を見たいので、"Experiment.RootClassLibrary" プロジェクトを依存関係に設定するのではなく
            // "Experiment.RootClassLibrary" から生成された Release 版のアセンブリを "参照" で追加すること。
            //  

            Console.WriteLine();
            Console.WriteLine($"----- {nameof(TestInlining)} -----");
            Console.WriteLine();

            var x = 20;
            var y = 3;

            // 同一クラス内の private メソッドは自動的にインライン化されるか?
            var resut1 = Calc.AddForSameClass(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{x} + 100 * {y} => {resut1}");

            // 同一クラス内の private クラスのメソッドは自動的にインライン化されるか?
            var resut2 = Calc.AddForPrivateClass(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{x} + 100 * {y} => {resut2}");

            // 同一アセンブリ内の internal クラス内の public メソッドは自動的にインライン化されるか?
            var resut3 = Calc.AddForInternalClass(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{x} + 100 * {y} => {resut3}");

            // 同一アセンブリ内の public クラス内の public メソッドは自動的にインライン化されるか?
            var resut4 = Calc.AddForPublicClass(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{x} + 100 * {y} => {resut4}");

            // 異なるアセンブリのメソッドは自動的にインライン化されるか?
            var resut5 = Calc.AddAcrossAssembly(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{x} + 100 * {y} => {resut5}");

            // インライン化属性が付加された、異なるアセンブリのメソッドはインライン化されるか?
            var resut6 = Calc.InlinedAddAcrossAssembly(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"{x} + 100 * {y} => {resut6}");

        }

        // ジェネリックメソッドに型パラメタとして struct を与えた場合、ジェネリックメソッド内での型パラメタへの null チェックは省略されるか?
        // => 型パラメタに class/struct のどちらも許容するジェネリックメソッドにおいて、呼び出し元が型パラメタに struct を与えた場合、ジェネリックメソッド内での型パラメタの null チェックは "常に非 null" であるように最適化される。
        // ジェネリックメソッド内で型パラメタによる分岐処理を行っていた場合、分岐処理は最適化されるか?
        // => 例えば型パラメタが 'TYPE_T' の場合に TYPE_T が ある特定の型と等しいかどうかのチェックを行う場合、"typeof(TYPE_T) == typeof(int)" のようにすると実行時に "常にtrue" または "常に false" であるように最適化される。
        //    同様に型パラメタのチェックを行う場合に Type.GetTypeCode(typeof(TYPE_T)) で取得できる TypeCode 列挙体による比較も可能ではあるが、こちらは最適化されず、型の比較コードがそのまま残ってしまう。
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestGenerics()
        {
            //
            // 注意:
            // Release版での結果を見たいので、"Experiment.RootClassLibrary" プロジェクトを依存関係に設定するのではなく
            // "Experiment.RootClassLibrary" から生成された Release 版のアセンブリを "参照" で追加すること。
            //  

            Console.WriteLine();
            Console.WriteLine($"----- {nameof(TestGenerics)} -----");
            Console.WriteLine();

            var x = 1;
            var y = 10;
            var min1 = ImplementationOfGenericMethods.Minimum1(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"Minimum1({x}, {y}) => {min1}");

#if NET7_0_OR_GREATER
            var min2 = ImplementationOfGenericMethods.Minimum2(x, y); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"Minimum1({x}, {y}) => {min2}");
#endif

            var sizeOfInt1 = ImplementationOfGenericMethods.GetSizeOfInt1(); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"sizeof(int) => {sizeOfInt1} (pattern 1)");

            var sizeOfInt2 = ImplementationOfGenericMethods.GetSizeOfInt2(); // <= ここにブレークポイントを設定して、停止したら逆アセンブリ画面で追跡する。
            Console.WriteLine($"sizeof(int) => {sizeOfInt2} (pattern 2)");
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static string GetStringParameter()
        {
            Console.WriteLine("Called 'GetStringParameter()'");
            return "Hello!";
        }
    }
}
