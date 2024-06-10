using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace ThinHull
{
    internal static class MyPluginInfo
    {
        internal const string PLUGIN_GUID = "id107.thinhull";
        internal const string PLUGIN_NAME = "Thin Hull";
        internal const string PLUGIN_VERSION = "0.0.0";
    }

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency("VoidManager")]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}