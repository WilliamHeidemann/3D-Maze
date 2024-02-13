using System;

namespace Game.CampaignMode
{
    public enum World
    {
        SmallWorld,
        Regular,
        Chunks,
        LongIsland,
        Massive
    }

    public static class WorldExtensionMethodClass
    {
        public static string AsString(this World world) =>
            world switch
            {
                World.SmallWorld => "Small World",
                World.Regular => "Regular",
                World.Chunks => "Chunks",
                World.LongIsland => "Long Island",
                World.Massive => "Massive",
                _ => throw new ArgumentOutOfRangeException(nameof(world), world, null)
            };
    }
}