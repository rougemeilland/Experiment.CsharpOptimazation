using System.Runtime.CompilerServices;

namespace Experiment.InternalClassLibrary
{
    public static class SimpleCalc
    {
        public static int Add(int x, int y) => x + y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AddWithInlineAttribute(int x, int y) => x + y;
    }
}
