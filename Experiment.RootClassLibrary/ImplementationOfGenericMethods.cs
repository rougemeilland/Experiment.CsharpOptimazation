using Experiment.InternalClassLibrary;

namespace Experiment.RootClassLibrary
{
    public class ImplementationOfGenericMethods
    {
        public static int Minimum1(int x, int y) => GenericMethods.Minimum1(x, y);
#if NET7_0_OR_GREATER
        public static int Minimum2(int x, int y) => GenericMethods.Minimum2(x, y);
#endif
        public static int GetSizeOfInt1() => GenericMethods.GetSizeOfType1<int>();
        public static int GetSizeOfInt2() => GenericMethods.GetSizeOfType2<int>();
    }
}
