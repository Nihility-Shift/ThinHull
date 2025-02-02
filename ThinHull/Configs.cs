using BepInEx.Configuration;

namespace ThinHull
{
    internal class Configs
    {
        internal static ConfigEntry<bool> minorLeak;

        internal static void Load(BepinPlugin plugin)
        {
            minorLeak = plugin.Config.Bind("ThinHull", "MinorsLeak", true);
        }
    }
}
