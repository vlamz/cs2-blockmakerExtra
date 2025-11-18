using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CS2TraceRay.Class;
using CS2TraceRay.Enum;
using CS2TraceRay.Struct;
using FixVectorLeak;
using System.Drawing;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

public static class SafeZone
{
    public class ZoneData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public VectorUtils.VectorDTO MinPosition { get; set; } = new();
        public VectorUtils.VectorDTO MaxPosition { get; set; } = new();
        public bool Godmode { get; set; } = true;
        public bool Healing { get; set; } = false;
        public float HealingAmount { get; set; } = 1.0f;
        public float HealingInterval { get; set; } = 1.0f;
        public bool Notify { get; set; } = true;
        public bool BlockDamageToOutside { get; set; } = true;
        public string Creator { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class SaveData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public VectorUtils.VectorDTO MinPosition { get; set; } = new();
        public VectorUtils.VectorDTO MaxPosition { get; set; } = new();
        public bool Godmode { get; set; } = true;
        public bool Healing { get; set; } = false;
        public float HealingAmount { get; set; } = 1.0f;
        public float HealingInterval { get; set; } = 1.0f;
        public bool Notify { get; set; } = true;
        public bool BlockDamageToOutside { get; set; } = true;
        public string Creator { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    private static Plugin Instance = Plugin.Instance;
    private static Config Config = Instance.Config;
    
    public class PendingZoneCreation
    {
        public Vector_t Position1 { get; set; }
        public Vector_t? Position2 { get; set; }
        public string ZoneName { get; set; } = "";
    }

    public static Dictionary<int, ZoneData> Zones = new();
    public static Dictionary<CCSPlayerController, PendingZoneCreation> PendingPositions = new();
    public static Dictionary<CCSPlayerController, Dictionary<int, Timer>> PlayerHealingTimers = new();
    public static Dictionary<CCSPlayerController, List<CounterStrikeSharp.API.Core.CBeam>> PreviewBeams = new();
    public static int NextZoneId = 1;

    public static void Initialize()
    {
        Zones.Clear();
        PendingPositions.Clear();
        PlayerHealingTimers.Clear();
        ClearAllPreviews();
        NextZoneId = 1;
    }

    public static void ClearPreview(CCSPlayerController player)
    {
        if (PreviewBeams.ContainsKey(player))
        {
            foreach (var beam in PreviewBeams[player])
            {
                if (beam != null && beam.IsValid)
                    beam.Remove();
            }
            PreviewBeams[player].Clear();
            PreviewBeams.Remove(player);
        }
    }

    public static void ClearAllPreviews()
    {
        foreach (var playerBeams in PreviewBeams.Values)
        {
            foreach (var beam in playerBeams)
            {
                if (beam != null && beam.IsValid)
                    beam.Remove();
            }
        }
        PreviewBeams.Clear();
    }

    public static void DrawZonePreview(CCSPlayerController player, Vector_t pos1, Vector_t? pos2 = null)
    {
        if (player == null || !player.IsValid)
            return;

        var color = Color.FromArgb(200, 0, 255, 0); // Semi-transparent green
        var width = 0.5f;

        // Clear existing preview
        ClearPreview(player);

        if (!PreviewBeams.ContainsKey(player))
            PreviewBeams[player] = new List<CounterStrikeSharp.API.Core.CBeam>();

        if (pos2 == null)
        {
            // Only first position - draw a small cross/marker
            var offset = 32.0f; // 32 units offset for visibility
            var beams = new[]
            {
                // Horizontal line
                new { Start = new Vector_t(pos1.X - offset, pos1.Y, pos1.Z), End = new Vector_t(pos1.X + offset, pos1.Y, pos1.Z) },
                // Vertical line
                new { Start = new Vector_t(pos1.X, pos1.Y - offset, pos1.Z), End = new Vector_t(pos1.X, pos1.Y + offset, pos1.Z) },
                // Up line
                new { Start = new Vector_t(pos1.X, pos1.Y, pos1.Z), End = new Vector_t(pos1.X, pos1.Y, pos1.Z + offset) }
            };

            foreach (var beam in beams)
            {
                var beamEntity = Utils.DrawBeam(beam.Start, beam.End, color, width);
                if (beamEntity != null)
                    PreviewBeams[player].Add(beamEntity);
            }
        }
        else
        {
            // Both positions - draw full zone preview
            var p1 = pos1;
            var p2 = pos2.Value;

            // Normalize min/max
            var min = new Vector_t(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Min(p1.Z, p2.Z)
            );
            var max = new Vector_t(
                Math.Max(p1.X, p2.X),
                Math.Max(p1.Y, p2.Y),
                Math.Max(p1.Z, p2.Z)
            );

            // Calculate 8 corners of the box
            var corners = new Vector_t[8]
            {
                new(min.X, min.Y, min.Z), // Bottom-back-left
                new(max.X, min.Y, min.Z), // Bottom-back-right
                new(max.X, max.Y, min.Z), // Bottom-front-right
                new(min.X, max.Y, min.Z), // Bottom-front-left
                new(min.X, min.Y, max.Z), // Top-back-left
                new(max.X, min.Y, max.Z), // Top-back-right
                new(max.X, max.Y, max.Z), // Top-front-right
                new(min.X, max.Y, max.Z)  // Top-front-left
            };

            // Draw 12 edges of the box
            var edges = new[]
            {
                // Bottom face
                (corners[0], corners[1]), (corners[1], corners[2]), (corners[2], corners[3]), (corners[3], corners[0]),
                // Top face
                (corners[4], corners[5]), (corners[5], corners[6]), (corners[6], corners[7]), (corners[7], corners[4]),
                // Vertical edges
                (corners[0], corners[4]), (corners[1], corners[5]), (corners[2], corners[6]), (corners[3], corners[7])
            };

            foreach (var (start, end) in edges)
            {
                var beamEntity = Utils.DrawBeam(start, end, color, width);
                if (beamEntity != null)
                    PreviewBeams[player].Add(beamEntity);
            }
        }
    }

    public static bool IsPlayerInZone(CCSPlayerController player, ZoneData zone)
    {
        if (player == null || !player.IsValid || player.PlayerPawn.Value == null)
            return false;

        var pawn = player.PlayerPawn.Value;
        if (pawn.AbsOrigin == null || !pawn.IsValid)
            return false;

        try
        {
            var pos = pawn.AbsOrigin.ToVector_t();
            
            // Validate position
            if (float.IsNaN(pos.X) || float.IsNaN(pos.Y) || float.IsNaN(pos.Z) ||
                float.IsInfinity(pos.X) || float.IsInfinity(pos.Y) || float.IsInfinity(pos.Z))
                return false;

            var min = new Vector_t(zone.MinPosition.X, zone.MinPosition.Y, zone.MinPosition.Z);
            var max = new Vector_t(zone.MaxPosition.X, zone.MaxPosition.Y, zone.MaxPosition.Z);

            // Validate zone boundaries
            if (float.IsNaN(min.X) || float.IsNaN(min.Y) || float.IsNaN(min.Z) ||
                float.IsNaN(max.X) || float.IsNaN(max.Y) || float.IsNaN(max.Z) ||
                float.IsInfinity(min.X) || float.IsInfinity(min.Y) || float.IsInfinity(min.Z) ||
                float.IsInfinity(max.X) || float.IsInfinity(max.Y) || float.IsInfinity(max.Z))
                return false;

            // Normalize min/max (min should be smaller, max should be larger)
            var actualMin = new Vector_t(
                Math.Min(min.X, max.X),
                Math.Min(min.Y, max.Y),
                Math.Min(min.Z, max.Z)
            );
            var actualMax = new Vector_t(
                Math.Max(min.X, max.X),
                Math.Max(min.Y, max.Y),
                Math.Max(min.Z, max.Z)
            );

            return pos.X >= actualMin.X && pos.X <= actualMax.X &&
                   pos.Y >= actualMin.Y && pos.Y <= actualMax.Y &&
                   pos.Z >= actualMin.Z && pos.Z <= actualMax.Z;
        }
        catch (Exception ex)
        {
            Utils.Log($"Error checking if player is in zone {zone.Id}: {ex.Message}");
            return false;
        }
    }

    public static ZoneData? GetPlayerZone(CCSPlayerController player)
    {
        foreach (var zone in Zones.Values)
        {
            if (IsPlayerInZone(player, zone))
                return zone;
        }
        return null;
    }

    public static void OnTick()
    {
        var players = Utilities.GetPlayers().Where(p => p.IsLegal() && p.IsAlive()).ToList();
        
        foreach (var player in players)
        {
            if (player == null || !player.IsValid)
                continue;

            var zone = GetPlayerZone(player);
            var pawn = player.PlayerPawn.Value;

            if (zone != null && pawn != null && pawn.IsValid)
            {
                // Apply godmode (only if not already in build mode godmode)
                bool buildModeGodmode = Instance.BuilderData.ContainsKey(player.Slot) && Instance.BuilderData[player.Slot].Godmode;
                if (!buildModeGodmode)
                {
                    if (zone.Godmode)
                    {
                        // Enable godmode if zone has it enabled
                        if (pawn.TakesDamage)
                            pawn.TakesDamage = false;
                    }
                    else
                    {
                        // Restore damage if zone has godmode disabled
                        if (!pawn.TakesDamage)
                            pawn.TakesDamage = true;
                    }
                }

                // Handle healing
                if (zone.Healing)
                {
                    if (!PlayerHealingTimers.ContainsKey(player))
                        PlayerHealingTimers[player] = new Dictionary<int, Timer>();

                    if (!PlayerHealingTimers[player].ContainsKey(zone.Id))
                    {
                        // Validate healing values
                        if (zone.HealingInterval <= 0 || float.IsNaN(zone.HealingInterval) || float.IsInfinity(zone.HealingInterval))
                        {
                            Utils.Log($"Invalid healing interval for zone {zone.Id}: {zone.HealingInterval}. Using default 1.0");
                            zone.HealingInterval = 1.0f;
                        }

                        if (zone.HealingAmount <= 0 || float.IsNaN(zone.HealingAmount) || float.IsInfinity(zone.HealingAmount))
                        {
                            Utils.Log($"Invalid healing amount for zone {zone.Id}: {zone.HealingAmount}. Using default 1.0");
                            zone.HealingAmount = 1.0f;
                        }

                        // Capture zone ID and player for closure safety
                        int zoneId = zone.Id;
                        float healingAmount = Math.Max(0.1f, zone.HealingAmount); // Ensure positive
                        float healingInterval = Math.Max(0.1f, zone.HealingInterval); // Ensure positive (minimum 0.1s)
                        var playerRef = player;
                        
                        var timer = Instance.AddTimer(healingInterval, () =>
                        {
                            try
                            {
                                if (playerRef == null || !playerRef.IsValid)
                                    return;

                                var playerPawn = playerRef.PlayerPawn.Value;
                                if (playerPawn == null || !playerPawn.IsValid || playerPawn.LifeState != (byte)LifeState_t.LIFE_ALIVE)
                                    return;

                                if (playerPawn.Health >= playerPawn.MaxHealth)
                                    return;

                                // Verify player is still in the zone
                                var currentZone = GetPlayerZone(playerRef);
                                if (currentZone == null || currentZone.Id != zoneId)
                                {
                                    // Player left zone, clean up timer
                                    if (playerRef != null && playerRef.IsValid && 
                                        PlayerHealingTimers.ContainsKey(playerRef) && 
                                        PlayerHealingTimers[playerRef].ContainsKey(zoneId))
                                    {
                                        PlayerHealingTimers[playerRef][zoneId]?.Kill();
                                        PlayerHealingTimers[playerRef].Remove(zoneId);
                                    }
                                    return;
                                }

                                var newHealth = Math.Min(playerPawn.Health + healingAmount, playerPawn.MaxHealth);
                                playerPawn.Health = (int)Math.Floor(newHealth);
                                Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
                            }
                            catch (Exception ex)
                            {
                                Utils.Log($"Error in healing timer for zone {zoneId}: {ex.Message}");
                            }
                        }, TimerFlags.REPEAT);

                        PlayerHealingTimers[player][zone.Id] = timer;
                    }
                }
            }
            else
            {
                // Player is not in any zone - restore normal damage
                if (pawn != null && pawn.IsValid && !pawn.TakesDamage)
                {
                    // Only restore if player is not in build mode with godmode enabled
                    bool buildModeGodmode = Instance.BuilderData.ContainsKey(player.Slot) && Instance.BuilderData[player.Slot].Godmode;
                    if (!buildModeGodmode)
                    {
                        pawn.TakesDamage = true;
                    }
                }

                // Clean up healing timers for this player
                if (PlayerHealingTimers.ContainsKey(player))
                {
                    foreach (var timer in PlayerHealingTimers[player].Values)
                    {
                        timer?.Kill();
                    }
                    PlayerHealingTimers[player].Clear();
                    PlayerHealingTimers.Remove(player);
                }
            }
        }

        // Clean up disconnected players from dictionaries
        var connectedPlayerSet = new HashSet<CCSPlayerController>(players);
        var disconnectedPlayers = PlayerHealingTimers.Keys.Where(p => !connectedPlayerSet.Contains(p)).ToList();
        foreach (var disconnectedPlayer in disconnectedPlayers)
        {
            foreach (var timer in PlayerHealingTimers[disconnectedPlayer].Values)
            {
                timer?.Kill();
            }
            PlayerHealingTimers.Remove(disconnectedPlayer);
        }

        var disconnectedPending = PendingPositions.Keys.Where(p => !connectedPlayerSet.Contains(p)).ToList();
        foreach (var disconnectedPlayer in disconnectedPending)
        {
            PendingPositions.Remove(disconnectedPlayer);
        }

        // Update previews for players with pending positions
        var disconnectedPreview = PreviewBeams.Keys.Where(p => !connectedPlayerSet.Contains(p)).ToList();
        foreach (var disconnectedPlayer in disconnectedPreview)
        {
            ClearPreview(disconnectedPlayer);
        }

        foreach (var player in players)
        {
            if (player == null || !player.IsValid)
                continue;

            if (PendingPositions.TryGetValue(player, out var pending) && pending != null)
            {
                try
                {
                    // Use CS2TraceRay to get crosshair position
                    var pawn = player.PlayerPawn.Value;
                    if (pawn == null || !pawn.IsValid)
                        continue;

                    CGameTrace? trace = TraceRay.TraceShape(player.GetEyePosition()!, pawn.EyeAngles, TraceMask.MaskShot, player);
                    if (trace != null && trace.HasValue && trace.Value.Position.Length() != 0)
                    {
                        var tracePos = trace.Value.Position;
                        var crosshairPos = new Vector_t(tracePos.X, tracePos.Y, tracePos.Z);
                        
                        // Validate position
                        if (!float.IsNaN(crosshairPos.X) && !float.IsNaN(crosshairPos.Y) && !float.IsNaN(crosshairPos.Z) &&
                            !float.IsInfinity(crosshairPos.X) && !float.IsInfinity(crosshairPos.Y) && !float.IsInfinity(crosshairPos.Z))
                        {
                            // Use Position2 if explicitly set by user, otherwise use current crosshair for preview only
                            // Do NOT modify pending.Position2 here - it should only be set by user commands
                            Vector_t? previewPos2 = pending.Position2 ?? crosshairPos;
                            
                            // Draw preview with first position and preview position (either explicit Position2 or current crosshair)
                            DrawZonePreview(player, pending.Position1, previewPos2);
                        }
                    }
                    else
                    {
                        // No valid trace - draw preview with stored positions
                        DrawZonePreview(player, pending.Position1, pending.Position2);
                    }
                }
                catch (Exception ex)
                {
                    Utils.Log($"Error updating SafeZone preview for player {player.Slot}: {ex.Message}");
                }
            }
        }
    }

    public static void NotifyPlayer(CCSPlayerController player, ZoneData zone, bool entered)
    {
        if (!zone.Notify || player == null || !player.IsValid)
            return;

        var message = entered
            ? $"{ChatColors.Green}Entered SafeZone: {ChatColors.White}{zone.Name}"
            : $"{ChatColors.Red}Left SafeZone: {ChatColors.White}{zone.Name}";

        Utils.PrintToChat(player, message);
    }

    public static int CreateZone(string name, Vector_t pos1, Vector_t pos2, CCSPlayerController? creator = null)
    {
        // Validate zone name
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Zone name cannot be empty");

        // Check for duplicate names (case-insensitive)
        if (Zones.Values.Any(z => z.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Zone with name '{name}' already exists");

        // Validate positions (prevent invalid/NaN positions)
        if (float.IsNaN(pos1.X) || float.IsNaN(pos1.Y) || float.IsNaN(pos1.Z) ||
            float.IsNaN(pos2.X) || float.IsNaN(pos2.Y) || float.IsNaN(pos2.Z) ||
            float.IsInfinity(pos1.X) || float.IsInfinity(pos1.Y) || float.IsInfinity(pos1.Z) ||
            float.IsInfinity(pos2.X) || float.IsInfinity(pos2.Y) || float.IsInfinity(pos2.Z))
        {
            throw new ArgumentException("Invalid zone position (NaN or Infinity)");
        }

        // Prevent integer overflow
        if (NextZoneId >= int.MaxValue)
        {
            // Reset to 1 if we've reached max, but check for conflicts
            NextZoneId = 1;
            while (Zones.ContainsKey(NextZoneId))
                NextZoneId++;
        }

        var zone = new ZoneData
        {
            Id = NextZoneId++,
            Name = name.Trim(),
            MinPosition = new VectorUtils.VectorDTO(pos1),
            MaxPosition = new VectorUtils.VectorDTO(pos2),
            Creator = creator?.PlayerName ?? "System",
            CreatedAt = DateTime.Now
        };

        Zones[zone.Id] = zone;
        return zone.Id;
    }

    public static bool DeleteZone(int id)
    {
        if (Zones.ContainsKey(id))
        {
            // Clean up healing timers for this zone
            foreach (var playerTimers in PlayerHealingTimers.Values)
            {
                if (playerTimers.ContainsKey(id))
                {
                    playerTimers[id]?.Kill();
                    playerTimers.Remove(id);
                }
            }

            Zones.Remove(id);
            return true;
        }
        return false;
    }

    public static ZoneData? GetZone(int id)
    {
        return Zones.TryGetValue(id, out var zone) ? zone : null;
    }

    public static ZoneData? GetZoneByName(string name)
    {
        return Zones.Values.FirstOrDefault(z => z.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public static bool CanPlayerDamage(CCSPlayerController attacker, CCSPlayerController victim)
    {
        try
        {
            if (attacker == null || !attacker.IsValid || victim == null || !victim.IsValid)
                return true; // Allow damage if we can't determine zones

            var attackerZone = GetPlayerZone(attacker);
            var victimZone = GetPlayerZone(victim);

            // If attacker is in a zone with BlockDamageToOutside enabled
            if (attackerZone != null && attackerZone.BlockDamageToOutside)
            {
                // If victim is not in the same zone, block damage
                if (victimZone == null || victimZone.Id != attackerZone.Id)
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Utils.Log($"Error checking if player can damage: {ex.Message}");
            return true; // Allow damage on error to avoid blocking legitimate damage
        }
    }
}

