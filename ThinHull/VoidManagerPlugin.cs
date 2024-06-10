using VoidManager.MPModChecks;

namespace ThinHull
{
    public class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Host;

        public override string Author => "18107";

        public override string Description => "Makes hull breaches leak air from the ship";
    }
}
