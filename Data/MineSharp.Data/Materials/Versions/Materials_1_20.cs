using MineSharp.Core.Common.Blocks;
using MineSharp.Core.Common.Items;

namespace MineSharp.Data.Materials.Versions;

internal class Materials_1_20 : MaterialVersion
{
    public override IDictionary<Material, Dictionary<ItemType, float>> Palette { get; } = new Dictionary<Material, Dictionary<ItemType, float>>()
    {
        {
            Material.Gourd,
            new Dictionary<ItemType, float>()
            {
                { ItemType.IronSword, 1.5f },
                { ItemType.NetheriteSword, 1.5f },
                { ItemType.WoodenSword, 1.5f },
                { ItemType.StoneSword, 1.5f },
                { ItemType.GoldenSword, 1.5f },
                { ItemType.DiamondSword, 1.5f },
            }
        },
        {
            Material.Wool,
            new Dictionary<ItemType, float>()
            {
                { ItemType.Shears, 5f },
            }
        },
        {
            Material.Axe,
            new Dictionary<ItemType, float>()
            {
                { ItemType.DiamondAxe, 8f },
                { ItemType.WoodenAxe, 2f },
                { ItemType.IronAxe, 6f },
                { ItemType.NetheriteAxe, 9f },
                { ItemType.StoneAxe, 4f },
                { ItemType.GoldenAxe, 12f },
            }
        },
        {
            Material.Cobweb,
            new Dictionary<ItemType, float>()
            {
                { ItemType.Shears, 15f },
                { ItemType.IronSword, 15f },
                { ItemType.NetheriteSword, 15f },
                { ItemType.WoodenSword, 15f },
                { ItemType.StoneSword, 15f },
                { ItemType.GoldenSword, 15f },
                { ItemType.DiamondSword, 15f },
            }
        },
        {
            Material.Default,
            new Dictionary<ItemType, float>()
            {
            }
        },
        {
            Material.Shovel,
            new Dictionary<ItemType, float>()
            {
                { ItemType.NetheriteShovel, 9f },
                { ItemType.WoodenShovel, 2f },
                { ItemType.DiamondShovel, 8f },
                { ItemType.IronShovel, 6f },
                { ItemType.StoneShovel, 4f },
                { ItemType.GoldenShovel, 12f },
            }
        },
        {
            Material.Leaves,
            new Dictionary<ItemType, float>()
            {
                { ItemType.Shears, 15f },
                { ItemType.IronSword, 1.5f },
                { ItemType.NetheriteSword, 1.5f },
                { ItemType.WoodenSword, 1.5f },
                { ItemType.StoneSword, 1.5f },
                { ItemType.GoldenSword, 1.5f },
                { ItemType.DiamondSword, 1.5f },
            }
        },
        {
            Material.Plant,
            new Dictionary<ItemType, float>()
            {
                { ItemType.IronSword, 1.5f },
                { ItemType.NetheriteSword, 1.5f },
                { ItemType.WoodenSword, 1.5f },
                { ItemType.StoneSword, 1.5f },
                { ItemType.GoldenSword, 1.5f },
                { ItemType.DiamondSword, 1.5f },
            }
        },
        {
            Material.Pickaxe,
            new Dictionary<ItemType, float>()
            {
                { ItemType.WoodenPickaxe, 2f },
                { ItemType.StonePickaxe, 4f },
                { ItemType.GoldenPickaxe, 12f },
                { ItemType.DiamondPickaxe, 8f },
                { ItemType.NetheritePickaxe, 9f },
                { ItemType.IronPickaxe, 6f },
            }
        },
        {
            Material.VineOrGlowLichen,
            new Dictionary<ItemType, float>()
            {
                { ItemType.Shears, 2f },
            }
        },
        {
            Material.Hoe,
            new Dictionary<ItemType, float>()
            {
                { ItemType.IronHoe, 6f },
                { ItemType.NetheriteHoe, 9f },
                { ItemType.StoneHoe, 4f },
                { ItemType.DiamondHoe, 8f },
                { ItemType.WoodenHoe, 2f },
                { ItemType.GoldenHoe, 12f },
            }
        },
    };
}