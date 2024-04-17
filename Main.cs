using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using Timer = System.Timers.Timer;

namespace GCD
{
    public class Main : Plugin<Config>
    {
        public override string Name => "Boombastic Comedy Countdown";
        public override string Author => "Akai";
        public override Version Version => new Version(1, 0, 5);

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
            Exiled.Events.Handlers.Player.ThrowingRequest += OnThrowingRequest;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
            Exiled.Events.Handlers.Player.ThrowingRequest -= OnThrowingRequest;

            base.OnDisabled();
        }

        /// <summary>
        /// System to handle the timers for the objects.
        /// </summary>
        private readonly Dictionary<Player, Timer> _timers = new Dictionary<Player, Timer>();
        private readonly Dictionary<Player, CancellationTokenSource> _tokenSources = new Dictionary<Player, CancellationTokenSource>();

        private void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
            if (Config.Debug)
            {
                Debug.Log($"OnThrowingRequest: {ev.RequestType}");
            }

            if (ev.RequestType == ThrowRequest.BeginThrow)
            {
                if (_tokenSources.ContainsKey(ev.Player))
                {
                    _tokenSources[ev.Player].Cancel();
                    _tokenSources[ev.Player].Dispose();
                }

                CancellationTokenSource tokenSource = new CancellationTokenSource();
                _tokenSources[ev.Player] = tokenSource;

                if (_timers.ContainsKey(ev.Player))
                {
                    _timers[ev.Player].Stop();
                    _timers[ev.Player].Dispose();
                }

                int timerDuration = 0;
                if (ev.Player.CurrentItem.Type == ItemType.GrenadeHE && Config.EnableGrenadeTimer)
                {
                    timerDuration = Config.GrenadeTimerDuration * 1000; // Fucker
                }
                else if (ev.Player.CurrentItem.Type == ItemType.GrenadeFlash && Config.EnableFlashTimer)
                {
                    timerDuration = Config.FlashTimerDuration * 1000; // Fucker
                }

                if (timerDuration > 0)
                {
                    Timer timer = new Timer(timerDuration);
                    timer.Elapsed += (sender, e) => OnTimerElapsed(ev.Player);
                    timer.AutoReset = false;
                    timer.Start();
                    
                    _timers[ev.Player] = timer;

                    // Start the countdown
                    CountdownTask(ev.Player, timerDuration / 1000, tokenSource.Token); // Fucker

                    if (Config.Debug)
                    {
                        Debug.Log($"Timer started for player {ev.Player.Nickname} with duration {timerDuration / 1000} seconds");
                    }
                }
            }
            else if (ev.RequestType == ThrowRequest.CancelThrow)
            {
                if (_tokenSources.ContainsKey(ev.Player))
                {
                    _tokenSources[ev.Player].Cancel();
                    _tokenSources[ev.Player].Dispose();
                    _tokenSources.Remove(ev.Player);
                }

                if (_timers.ContainsKey(ev.Player))
                {
                    _timers[ev.Player].Stop();
                    _timers[ev.Player].Dispose();
                    _timers.Remove(ev.Player);
                }
            }
        }

        private void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (_timers.ContainsKey(ev.Player))
            {
                _timers[ev.Player].Stop();
                _timers[ev.Player].Dispose();
                _timers.Remove(ev.Player);
            }
        }

        private async Task CountdownTask(Player player, int countdown, CancellationToken token)
        {
            while (countdown > 0)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                string hintMessage = player.CurrentItem.Type == ItemType.GrenadeHE ? Config.GrenadeExplodeText : Config.FlashExplodeText;
                player.ShowHint(string.Format(hintMessage, countdown--), 1f);

                if (Config.Debug)
                {
                    Debug.Log($"Countdown for player {player.Nickname}: {countdown} seconds remaining");
                }

                await Task.Delay(1000, token); // Wait 1 sec
            }
        }

        private void OnTimerElapsed(Player player)
        {
            if (player.CurrentItem.Type == ItemType.GrenadeHE)
            {
                player.RemoveHeldItem();
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = Config.GrenadeFuseTime;
                grenade.SpawnActive(player.Position, player); // owner of object
                grenade.ConcussDuration = Config.GrenadeConcussDuration;
                grenade.BurnDuration = Config.GrenadeBurnDuration;
            }
            else if (player.CurrentItem.Type == ItemType.GrenadeFlash)
            {
                player.RemoveHeldItem();
                FlashGrenade flash = (FlashGrenade)Item.Create(ItemType.GrenadeFlash);
                flash.FuseTime = Config.FlashFuseTime;
                flash.SpawnActive(player.Position, player); // owner of object
            }
            if (_timers.ContainsKey(player))
            {
                _timers[player].Dispose();
                _timers.Remove(player);
                if (Config.Debug)
                {
                    Debug.Log($"Timer elapsed for player {player.Nickname}");
                }
            }
        }
    }
}