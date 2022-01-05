using System;

namespace HeartBitSimulation
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new HBSimGame())
                game.Run();
        }
    }
}
