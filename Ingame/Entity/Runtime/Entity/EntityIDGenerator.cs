namespace Ingame
{
    public static class EntityIDGenerator
    {
        private static int next = 0;

        public static int Generate() => next++;
        public static void Reset() => next = 0;
    }
}
