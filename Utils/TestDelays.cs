using System;
using System.Threading;

namespace SauceDemoTests.Utils
{
    public static class TestDelays
    {
        public static int StepDelayMs = 1200;

        public static void Pause()
        {
            if (StepDelayMs > 0)
                Thread.Sleep(StepDelayMs);
        }
    }
}