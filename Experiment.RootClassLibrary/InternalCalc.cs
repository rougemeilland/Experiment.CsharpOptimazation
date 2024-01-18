using System.Runtime.CompilerServices;

namespace Experiment.RootClassLibrary
{
    internal class InternalCalc
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Add(int x, int y) => x + y;
    }
}
