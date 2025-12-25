using Exiled.API.Features;
using System.Collections.Generic;
using PlayerEventArgs = Exiled.Events.EventArgs.Player;
using MapEventArgs = Exiled.Events.EventArgs.Map;
using ServerEventArgs = Exiled.Events.EventArgs.Server;
using Exiled.API.Features.Items;
using Exiled.API.Structs;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem;

namespace Difficulty
{
    public class Special : WaveConfig
    {
        public override bool IsSpecial { get; } = true;
        public override int Difficulty { get; } = 2;
        public override string DifficultyName { get; } = "<color=#c0c0ff>???</color>";

        public override void OnThrownProjectile(PlayerEventArgs.ThrownProjectileEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Snowball)
            {
                ev.Player.Inventory.ServerAddItem(ItemType.Snowball, InventorySystem.Items.ItemAddReason.AdminCommand);
            }
        }

        public override void OnHurting(PlayerEventArgs.HurtingEventArgs ev)
        {
            if (ev.Player == null) return;
            if (ev.Attacker == null) return;
            if (ev.Attacker == ev.Player)
            {
                ev.IsAllowed = false; return;
            }
            if (!ev.Attacker.IsNPC)
            {
                if (ev.DamageHandler.Type == Exiled.API.Enums.DamageType.SnowBall)
                {
                    if (ev.DamageHandler.Damage > 25)
                    {
                        //Map.Explode(ev.Player.Position, Exiled.API.Enums.ProjectileType.FragGrenade, ev.Attacker);
                        Throwable throwable = ev.Attacker.ThrowGrenade(Exiled.API.Enums.ProjectileType.FragGrenade, false);
                        ExplosiveGrenade grenade = throwable as ExplosiveGrenade;
                        grenade.FuseTime = 0f;
                        grenade.Projectile.Position = ev.Player.Position;
                        return;
                    }
                    ev.DamageHandler.Damage *= 6.5f; return;
                }
                return;
            }
            if (ev.Player.IsNPC) { ev.IsAllowed = false; return; }
            if (ev.Attacker.CurrentItem == null)
            {
                if (ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp939) ev.DamageHandler.Damage *= 1.5f;
                return;
            }

            switch (ev.Attacker.CurrentItem.Type)
            {
                case ItemType.Jailbird: ev.DamageHandler.Damage *= 0.5f; break;
                case ItemType.MicroHID: ev.DamageHandler.Damage *= 0.3f; break;
                case ItemType.GunLogicer: ev.DamageHandler.Damage *= 0.9f; break;
                case ItemType.ParticleDisruptor: ev.DamageHandler.Damage *= 0.5f; break;

                case ItemType.GunCrossvec: ev.DamageHandler.Damage *= 0.5f; break;
                case ItemType.GunA7: ev.DamageHandler.Damage *= 0.35f; break;
                case ItemType.GunAK: ev.DamageHandler.Damage *= 0.6f; break;
                default: ev.DamageHandler.Damage *= 0.6f; break;
            }
        }

        /*
       ClassD: 3, 30
       Gunner: 2, 24
       Scout: 2, 24
       Cloaker: 1, 4
       Zombie: 2, 16
       Rifleman: 1, 15
       Tranquilizer: 1, 6
       Pyromancer: 1, 5
       Striker: 1, 3
       Juggernaut: 1, 4
       Demolisher: 0, 0
       Sniper: 1, 7
       Assassin: 1, 1
        */

        public override WaveInfo[] Waves { get; } = new WaveInfo[3]//웨이브 구조
        {
            new WaveInfo(
                    intermissionTime: 10,
                    bcText: "웨이브 1",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.ArmorHeavy, 1, 10),
                        new SupplySpawnInfo(ItemType.SCP1507Tape, 1, 8),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("ClassD", 5, 60),
                    },
                    supplyGiveInfos: new List<ItemType>(){ItemType.Snowball, ItemType.Coal},
                    maxEnemyCount:5,
                    maxMaxEnemyCount:20,
                    minEnemyCount:1,
                    maxMinEnemyCount:8
            ),
            new WaveInfo(
                    intermissionTime: 10,
                    bcText: "웨이브 2",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Gunner", 1, 14),
                        new EnemySpawnInfo("Zombie", 1, 6),
                        new EnemySpawnInfo("Cloaker", 0.5f, 2),
                    },
                    supplyGiveInfos: new List<ItemType>(){ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Medkit},
                    maxEnemyCount:5,
                    maxMaxEnemyCount:15,
                    minEnemyCount:1,
                    maxMinEnemyCount:6
            ),
            new WaveInfo(
                    intermissionTime: 10,
                    bcText: "<color=\"red\">마지막 웨이브</color>",
                    supplySpawnInfos: new List<SupplySpawnInfo>
                    {
                        new SupplySpawnInfo(ItemType.SCP1344, 0.8f, 5),
                        new SupplySpawnInfo(ItemType.AntiSCP207, 1, 5),
                        new SupplySpawnInfo(ItemType.SCP500, 2, 12),
                        new SupplySpawnInfo(ItemType.Medkit, 2, 8),
                        new SupplySpawnInfo(ItemType.SCP268, 1, 6),
                    },
                    enemySpawnInfos: new List<EnemySpawnInfo>
                    {
                        new EnemySpawnInfo("Gunner", 1, 16),
                        new EnemySpawnInfo("Zombie", 1, 6),
                        new EnemySpawnInfo("Cloaker", 0.5f, 2),
                    },
                    supplyGiveInfos: new List<ItemType>(){ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.Medkit},
                    maxEnemyCount:5,
                    maxMaxEnemyCount:15,
                    minEnemyCount:1,
                    maxMinEnemyCount:6
            )
        };
    }
}