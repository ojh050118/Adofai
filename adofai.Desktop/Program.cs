using osu.Framework.Platform;
using osu.Framework;
using adofai.Game;

namespace adofai.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"adofai"))
            using (osu.Framework.Game game = new adofaiGame())
                host.Run(game);
        }
    }
}
