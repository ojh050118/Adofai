using osu.Framework.Platform;
using osu.Framework;
using Adofai.Game;

namespace Adofai.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"Adofai"))
            using (osu.Framework.Game game = new AdofaiGame())
                host.Run(game);
        }
    }
}
