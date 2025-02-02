using UnityEngine;
using VoidManager.CustomGUI;

namespace ThinHull
{
    internal class GUI : ModSettingsMenu
    {
        public override string Name() => "Thin Hull";

        public override void Draw()
        {
            if (GUILayout.Button($"Minor breaches leak: {(Configs.minorLeak.Value ? "On" : "Off")}"))
            {
                Configs.minorLeak.Value = !Configs.minorLeak.Value;
            }
        }
    }
}
