using System.Runtime.CompilerServices;

namespace Experiment.RootClassLibrary
{
    public static class PublicCalc
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Add(int x, int y) => x + y;
    }
}
