namespace Experiment.RootClassLibrary
{
    public static class OptimizeMulAndDiv
    {
        private const int _CONSTANT_1024 = 1024;
        private const uint _CONSTANT_UNSIGNED_1024 = 1024;

        public static int MultiplyBy1024(int value) => value * _CONSTANT_1024;
        public static int DivideBy1024(int value) => value / _CONSTANT_1024;
        public static int RemainderAt1024(int value) => value % _CONSTANT_1024;
        public static uint UnsignedMultiplyBy1024(uint value) => value * _CONSTANT_UNSIGNED_1024;
        public static uint UnsignedDivideBy1024(uint value) => value / _CONSTANT_UNSIGNED_1024;
        public static uint UnsignedRemainderAt1024(uint value) => value % _CONSTANT_UNSIGNED_1024;
    }
}
