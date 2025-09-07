using Exiled.API.Features;
using System.Collections.Generic;
using PlayerEventArgs = Exiled.Events.EventArgs.Player;
using MapEventArgs = Exiled.Events.EventArgs.Map;
using ServerEventArgs = Exiled.Events.EventArgs.Server;
using Exiled.API.Features.Items;

public class WaveConfig
{
    public int MulCount { get; set; } = 0;
    //public float EnemyMultiplyPerPlayers { get; } = 0.25f;//플레이어당 유닛 배율(2인 이상에서만 유효) --안씀
    public float SupplyMultiplyPerPlayers { get; } = 0.25f;//플레이어당 보급 배율(2인 이상에서만 유효)

    public void OnHurting(PlayerEventArgs.HurtingEventArgs ev)
    {
        if (ev.Player == null) return;
        if (ev.Attacker == null) return;
        if (!ev.Attacker.IsNPC) return;
        if (ev.Player.IsNPC) { ev.IsAllowed = false; return; }
        if (ev.Player.CurrentItem == null) return;

        switch (ev.Attacker.CurrentItem.Type)
        {
            case ItemType.Jailbird: ev.DamageHandler.Damage *= 0.5f; break;//항상 25
            case ItemType.MicroHID: ev.DamageHandler.Damage *= (0.15f + 0.01f * MulCount); break;//35명 -> X0.5
            case ItemType.GunCrossvec: ev.DamageHandler.Damage *= (0.1f + 0.02f * MulCount); break;//35명 -> X0.8
            case ItemType.GunLogicer: ev.DamageHandler.Damage *= (0.2f + 0.02f * MulCount); break;//35명 -> X0.9
            default: ev.DamageHandler.Damage *= (0.08f + 0.01f * MulCount); break;//35명 -> X0.43
        }
    }
    public void OnPickupAdded(MapEventArgs.PickupAddedEventArgs ev)
    {
        if (ev.Pickup.PreviousOwner == null) return;
        if (!ev.Pickup.PreviousOwner.IsNPC) return;
        ev.Pickup.Destroy();
    }
    public void OnPlacingBulletHole(MapEventArgs.PlacingBulletHoleEventArgs ev) { ev.IsAllowed = false; }
    public void OnRespawningTeam(ServerEventArgs.RespawningTeamEventArgs ev) { ev.IsAllowed = false; }

    public WaveInfo[] Waves { get; } = new WaveInfo[15]//웨이브 구조
    {
            new WaveInfo(
                    intermissionTime: 10,
                    bcText: "Wave 1",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.GunFSP9, 1),
                        new SupplySpawnInfo(ItemType.ArmorLight, 1),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("ClassD", 4, 35),
                    }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 2",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.GunCrossvec, 1),
                        new SupplySpawnInfo(ItemType.ArmorLight, 1),
                        new SupplySpawnInfo(ItemType.SCP1344, 1),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 1, 5),
                        new EnemySpawnInfo("ClassD", 3, 30),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.Medkit, ItemType.Painkillers, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 3",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.GunCrossvec, 2),
                        new SupplySpawnInfo(ItemType.ArmorCombat, 2),
                        new SupplySpawnInfo(ItemType.SCP1344, 2),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 1, 5),
                        new EnemySpawnInfo("ClassD", 3, 10),
                        new EnemySpawnInfo("Scout", 1, 10),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 4",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.Adrenaline, 3),
                        new SupplySpawnInfo(ItemType.GunCrossvec, 2),
                        new SupplySpawnInfo(ItemType.ArmorHeavy, 1),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 2, 8),
                        new EnemySpawnInfo("ClassD", 4, 15),
                        new EnemySpawnInfo("Scout", 2, 15),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.Medkit, ItemType.Adrenaline, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 20,
                    bcText: "<color=\"red\">Wave 5</color>",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP268, 1),
                        new SupplySpawnInfo(ItemType.GunE11SR, 2),
                        new SupplySpawnInfo(ItemType.ArmorHeavy, 2),
                        new SupplySpawnInfo(ItemType.SCP1344, 1),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("ClassD", 5, 5),
                        new EnemySpawnInfo("Scout", 4, 12),
                        new EnemySpawnInfo("Pyromancer", 1, 3.5f),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.SCP500, ItemType.Adrenaline, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 6",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP500, 1),
                        new SupplySpawnInfo(ItemType.SCP268, 1),
                        new SupplySpawnInfo(ItemType.GunLogicer, 1),
                        new SupplySpawnInfo(ItemType.ArmorHeavy, 1),
                        new SupplySpawnInfo(ItemType.Ammo762x39, 2),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 1, 3.8f),
                        new EnemySpawnInfo("Scout", 6, 25),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunCrossvec, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 7",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP500, 1),
                        new SupplySpawnInfo(ItemType.Ammo762x39, 2),
                        new SupplySpawnInfo(ItemType.ArmorHeavy, 1),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 1, 5),
                        new EnemySpawnInfo("Scout", 6, 25),
                        new EnemySpawnInfo("Tranquilizer", 1, 5),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 8",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.GunLogicer, 1),
                        new SupplySpawnInfo(ItemType.SCP500, 1),
                        new SupplySpawnInfo(ItemType.Ammo762x39, 4),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 1, 5),
                        new EnemySpawnInfo("Scout", 6, 35),
                        new EnemySpawnInfo("Tranquilizer", 1, 5),
                        new EnemySpawnInfo("Pyromancer", 1, 1),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunCrossvec, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 9",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.GunSCP127, 1),
                        new SupplySpawnInfo(ItemType.SCP500, 1),
                        new SupplySpawnInfo(ItemType.SCP1344, 1),
                        new SupplySpawnInfo(ItemType.Ammo762x39, 2),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 1, 5),
                        new EnemySpawnInfo("Scout", 6, 35),
                        new EnemySpawnInfo("Tranquilizer", 1, 6),
                        new EnemySpawnInfo("Pyromancer", 1, 3.5f),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunAK, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 20,
                    bcText: "<color=\"red\">Wave 10</color>",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.ArmorCombat, 2),
                        new SupplySpawnInfo(ItemType.ArmorHeavy, 1),
                        new SupplySpawnInfo(ItemType.Ammo762x39, 10),
                        new SupplySpawnInfo(ItemType.SCP268, 1),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Scout", 10, 40),
                        new EnemySpawnInfo("Juggernaut", 1, 2.8f),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunAK, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 11",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP268, 1),
                        new SupplySpawnInfo(ItemType.SCP500, 2),
                        new SupplySpawnInfo(ItemType.GunE11SR, 2),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 2, 8),
                        new EnemySpawnInfo("Scout", 10, 30),
                        new EnemySpawnInfo("Pyromancer", 1, 2.8f),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunCrossvec, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 12",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP500, 1),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 2, 8),
                        new EnemySpawnInfo("Scout", 10, 30),
                        new EnemySpawnInfo("Demolisher", 1, 2.9f),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunCrossvec, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 13",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP500, 1),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 2, 8),
                        new EnemySpawnInfo("Scout", 10, 30),
                        new EnemySpawnInfo("Demolisher", 1, 3.8f),
                        new EnemySpawnInfo("Tranquilizer", 2, 10),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunCrossvec, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 15,
                    bcText: "Wave 14",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP500, 3),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 3, 8),
                        new EnemySpawnInfo("Scout", 10, 30),
                        new EnemySpawnInfo("Demolisher", 1, 3.8f),
                        new EnemySpawnInfo("Tranquilizer", 2, 10),
                        new EnemySpawnInfo("Pyromancer", 2, 5),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunCrossvec, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45, ItemType.Ammo9x19, ItemType.Ammo9x19 }
            ),
            new WaveInfo(
                    intermissionTime: 30,
                    bcText: "<color=\"red\">마지막 웨이브</color>",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.GunLogicer, 1),
                        new SupplySpawnInfo(ItemType.SCP500, 1),
                        new SupplySpawnInfo(ItemType.Ammo762x39, 2),
                        new SupplySpawnInfo(ItemType.SCP268, 1),

                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Cloaker", 2, 6),
                        new EnemySpawnInfo("Scout", 5, 20),
                        new EnemySpawnInfo("Demolisher", 1, 3.8f),
                        new EnemySpawnInfo("Tranquilizer", 1, 5),
                        new EnemySpawnInfo("Pyromancer", 2, 5),
                        new EnemySpawnInfo("Juggernaut", 2, 7),
                        new EnemySpawnInfo("Scout", 5, 20),
                    },
                    supplyGiveInfos: new List<ItemType>(){ ItemType.GunE11SR, ItemType.Medkit, ItemType.Ammo9x19, ItemType.Ammo556x45 }
            ),
    };
    //타입
    public class EnemySpawnInfo
    {
        public string EnemyName { get; }
        public float Amount { get; }
        public float EnemyPerPlayer { get; }
        public EnemySpawnInfo(string enemyType, float amount = 1, float maxAmount = 1)
        {
            EnemyName = enemyType;
            Amount = amount;
            if (amount > maxAmount)
            {
                EnemyPerPlayer = 0;
            }
            else
            {
                EnemyPerPlayer = (maxAmount - amount) / 34f;
            }
        }
    }
    public class SupplySpawnInfo
    {
        public ItemType Type { get; }
        public float Amount { get; }

        public SupplySpawnInfo(ItemType type, float amount)
        {
            Type = type;
            Amount = amount;
        }
    }
    public class SupplyGiveInfo
    {
        public ItemType Type { get; }

        public SupplyGiveInfo(ItemType type)
        {
            Type = type;
        }
    }
    public class WaveInfo
    {
        public List<EnemySpawnInfo> EnemySpawnInfos { get; }
        public int IntermissionTime { get; }
        public List<SupplySpawnInfo> SupplySpawnInfos { get; }
        public List<ItemType> SupplyGiveInfos { get; }

        public string BCtext { get; }

        public WaveInfo(
            List<EnemySpawnInfo> enemySpawnInfos,
            int intermissionTime,
            List<SupplySpawnInfo> supplySpawnInfos,
            string bcText,
            List<ItemType> supplyGiveInfos = null)
        {
            IntermissionTime = intermissionTime;
            SupplySpawnInfos = supplySpawnInfos;
            EnemySpawnInfos = enemySpawnInfos;
            BCtext = bcText;
            SupplyGiveInfos = supplyGiveInfos;

        }
    }
}