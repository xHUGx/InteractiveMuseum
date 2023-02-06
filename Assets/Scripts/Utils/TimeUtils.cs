namespace ParticleInk.TedAr.Utils
{
    public static class TimeUtils
    {
        private const float MillisecondsInTick = 10000000f;
        
        public static long MillisecondToTick(float millisecond)
        {
            return (long) (millisecond * MillisecondsInTick);
        }

        public static float TickToMillisecond(long tick)
        {
            return tick / MillisecondsInTick;
        }
            
    }
}