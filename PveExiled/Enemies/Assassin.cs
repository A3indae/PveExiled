using Exiled.API.Features.Pickups;
using Exiled.API.Features.Items;
using MEC;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using System.Linq;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem;
using InventorySystem.Items;
using Exiled.API.Features;
using System.Collections.Generic;
using SemanticVersioning;
using InventorySystem.Items.Firearms.Modules.Misc;
using Mirror;
using Exiled.API.Features.Roles;
using NetworkManagerUtils.Dummies;
using Exiled.Events.Handlers;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Respawning.Config;

namespace PveExiled.Enemies
{
    public class Assassin : Enemy
    {
        float fireRate = 0.68f;
        float updateDuration = 0.1f;
        bool canShoot = true;
        float moveBackMinDist = 0.8f;
        float selfMaxHealth;

        CoroutineHandle followootine;
        CoroutineHandle targetSetRootine;
        CoroutineHandle enemyRootine;

        DummyAction? shootAction;

        public Assassin(string enemyName, Vector3 spawnPos, int id, Dictionary<int, Enemy> container, WaveConfig waveConfig) : base(enemyName, spawnPos, id, container, waveConfig)
        {
            range = 1.6f;
            pathCompCheckTime = 0.15f;
            range = range * range;
            moveBackMinDist = moveBackMinDist * moveBackMinDist;
            selfPlayer.Role.Set(PlayerRoles.RoleTypeId.Scp939, SpawnReason.ForceClass, PlayerRoles.RoleSpawnFlags.All);
            foreach(Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                if (player != null && player.Role.Type == PlayerRoles.RoleTypeId.NtfSpecialist)
                {
                    player.EnableEffect<AmnesiaVision>(0, false);
                }
            }
            selfPlayer.EnableEffect<SpawnProtected>(1, true);
            selfPlayer.MaxHumeShield = 0;
            selfMaxHealth = 200 + waveConfig.MulCount * 10;
            selfPlayer.MaxHealth = 200 + selfMaxHealth;//30명 -> 500HP
            selfPlayer.Health = 200 + selfMaxHealth;
            fpc = selfPlayer.RoleManager.CurrentRole as IFpcRole;

            selfPlayer.Position = spawnPos;
            Timing.CallDelayed(0.5f, () => {
                if (removed) return;
                LoadAction();
                Timing.CallDelayed(1f, () =>
                {
                    if (removed) return;
                    targetSetRootine = Timing.RunCoroutine(RerollTarget());
                    followootine = Timing.RunCoroutine(FollowLoop());
                    enemyRootine = Timing.RunCoroutine(EnemyFunction());
                });
            });
        }
        public override void RemoveEnemy()
        {
            if (removed) return;
            removed = true;
            Timing.KillCoroutines(targetSetRootine);
            Timing.KillCoroutines(followootine);
            Timing.KillCoroutines(enemyRootine);

            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                if (player != null && player.Role.Type == PlayerRoles.RoleTypeId.NtfSpecialist)
                {
                    player.DisableEffect<AmnesiaVision>();
                }
            }

            base.RemoveEnemy();//인벤클리어&이벤트끊기&리스트제거
        }

        private void LoadAction()
        {
            selfPlayer.Role.Set(PlayerRoles.RoleTypeId.Scp939, SpawnReason.ForceClass, PlayerRoles.RoleSpawnFlags.None);
            selfPlayer.EnableEffect<SpawnProtected>(1, true);
            selfPlayer.MaxHumeShield = 0;
            selfPlayer.MaxHealth = selfMaxHealth;//30명 -> 500HP
            selfPlayer.Health = selfMaxHealth;
            Timing.CallDelayed(0.5f, () =>
            {
                if (removed) return;
                foreach (var a in DummyActionCollector.ServerGetActions(hub))
                {
                    if (a.Name.EndsWith("Shoot->Click")) { shootAction = a; break; }
                }
            });
        }

        private IEnumerator<float> EnemyFunction()
        {
            while (!removed)
            {
                yield return Timing.WaitForSeconds(updateDuration);
                FollowAndLook();
                if (!canShoot) continue;
                if (targetPlayer == null) continue;

                Vector3 lookDirection = targetPlayer.Position - selfPlayer.Position;
                if (lookDirection.sqrMagnitude > 0)
                {
                    if (lookDirection.sqrMagnitude > range) continue;
                    bool shootCast = Physics.Raycast(selfPlayer.Position, lookDirection.normalized, out RaycastHit _, maxDistance: lookDirection.magnitude, layerMask: mask, queryTriggerInteraction: QueryTriggerInteraction.Ignore);
                    if (shootCast) continue;

                    canShoot = false;

                    //Server.ExecuteCommand($"/dummy action {selfPlayer.Id}. GunCOM18_(#{firearm.ItemSerial}) Shoot->Click");
                    if (!shootAction.HasValue)
                    {
                        LoadAction();
                        continue;
                    }
                    shootAction.Value.Action();

                    Timing.CallDelayed(fireRate, () => canShoot = true);
                    if (followEnabled && lookDirection.sqrMagnitude < moveBackMinDist)
                    {
                        followEnabled = false;
                        Timing.CallDelayed(0.5f, () => { if (removed) return; followEnabled = true; });
                    }
                }
            }
        }
    }
}
