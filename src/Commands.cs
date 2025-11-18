using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using FixVectorLeak;

public static class Commands
{
    private static Plugin Instance = Plugin.Instance;
    private static Config config = Instance.Config;
    private static Config_Commands commands = Instance.Config.Commands;
    private static Dictionary<int, Building.BuilderData> BuilderData = Instance.BuilderData;

    public static void Load()
    {
        AddCommands(commands.Admin.BuildMode, BuildMode);
        AddCommands(commands.Admin.ManageBuilder, ManageBuilder);
        AddCommands(commands.Admin.ResetProperties, ResetProperties);
        AddCommands(commands.Building.BuildMenu, BuildMenu);
        AddCommands(commands.Building.BlockType, BlockType);
        AddCommands(commands.Building.BlockColor, BlockColor);
        AddCommands(commands.Building.CreateBlock, CreateBlock);
        AddCommands(commands.Building.DeleteBlock, DeleteBlock);
        AddCommands(commands.Building.RotateBlock, RotateBlock);
        AddCommands(commands.Building.PositionBlock, PositionBlock);
        AddCommands(commands.Building.SaveBlocks, SaveBlocks);
        AddCommands(commands.Building.Snapping, Snapping);
        AddCommands(commands.Building.Grid, Grid);
        AddCommands(commands.Building.Noclip, Noclip);
        AddCommands(commands.Building.Godmode, Godmode);
        AddCommands(commands.Building.TestBlock, TestBlock);
        AddCommands(commands.Building.ConvertBlock, ConvertBlock);
        AddCommands(commands.Building.CopyBlock, CopyBlock);
        AddCommands(commands.Building.LockBlock, LockBlock);
        AddCommands(commands.Building.LockAll, LockAll);
        
        // SafeZone commands
        AddCommands(commands.SafeZone.Create, CreateSafeZone);
        AddCommands(commands.SafeZone.List, ListSafeZones);
        AddCommands(commands.SafeZone.Delete, DeleteSafeZone);
    }
    private static void AddCommands(List<string> commands, Action<CCSPlayerController?, string> action)
    {
        foreach (var cmd in commands)
            Instance.AddCommand($"css_{cmd}", "", (player, command) => action(player, command.ArgString));
    }
    private static void AddCommands(List<string> commands, Action<CCSPlayerController?> action)
    {
        foreach (var cmd in commands)
            Instance.AddCommand($"css_{cmd}", "", (player, command) => action(player));
    }

    public static void Unload()
    {
        RemoveCommands(commands.Admin.BuildMode, BuildMode);
        RemoveCommands(commands.Admin.ManageBuilder, ManageBuilder);
        RemoveCommands(commands.Admin.ResetProperties, ResetProperties);
        RemoveCommands(commands.Building.BuildMenu, BuildMenu);
        RemoveCommands(commands.Building.BlockType, BlockType);
        RemoveCommands(commands.Building.BlockColor, BlockColor);
        RemoveCommands(commands.Building.CreateBlock, CreateBlock);
        RemoveCommands(commands.Building.DeleteBlock, DeleteBlock);
        RemoveCommands(commands.Building.RotateBlock, RotateBlock);
        RemoveCommands(commands.Building.PositionBlock, PositionBlock);
        RemoveCommands(commands.Building.SaveBlocks, SaveBlocks);
        RemoveCommands(commands.Building.Snapping, Snapping);
        RemoveCommands(commands.Building.Grid, Grid);
        RemoveCommands(commands.Building.Noclip, Noclip);
        RemoveCommands(commands.Building.Godmode, Godmode);
        RemoveCommands(commands.Building.TestBlock, TestBlock);
        RemoveCommands(commands.Building.ConvertBlock, ConvertBlock);
        RemoveCommands(commands.Building.CopyBlock, CopyBlock);
        RemoveCommands(commands.Building.LockBlock, LockBlock);
        RemoveCommands(commands.Building.LockAll, LockAll);
        
        // SafeZone commands
        RemoveCommands(commands.SafeZone.Create, CreateSafeZone);
        RemoveCommands(commands.SafeZone.List, ListSafeZones);
        RemoveCommands(commands.SafeZone.Delete, DeleteSafeZone);
    }
    private static void RemoveCommands(List<string> commands, Action<CCSPlayerController?, string> action)
    {
        foreach (var cmd in commands)
            Instance.RemoveCommand($"css_{cmd}", (player, command) => action(player, command.ArgString));
    }
    private static void RemoveCommands(List<string> commands, Action<CCSPlayerController?> action)
    {
        foreach (var cmd in commands)
            Instance.RemoveCommand($"css_{cmd}", (player, command) => action(player));
    }

    private static bool AllowedCommand(CCSPlayerController? player)
    {
        if (player == null || player.NotValid())
            return false;

        if (!Utils.BuildMode(player))
            return false;

        return true;
    }

    private static void ToggleCommand(CCSPlayerController player, ref bool commandStatus, string commandName)
    {
        commandStatus = !commandStatus;

        string status = commandStatus ? "ON" : "OFF";
        char color = commandStatus ? ChatColors.Green : ChatColors.Red;

        Utils.PrintToChat(player, $"{commandName}: {color}{status}");
    }

    public static void BuildMode(CCSPlayerController? player)
    {
        if (player == null || player.NotValid())
            return;

        if (!Utils.HasPermission(player))
        {
            Utils.PrintToChatAll($"{ChatColors.Red}You don't have permission to change Build Mode");
            return;
        }

        if (!Instance.buildMode)
        {
            Instance.buildMode = true;
            foreach (var target in Utilities.GetPlayers().Where(p => !p.IsBot))
            {
                if (Utils.HasPermission(target) || Files.Builders.steamids.Contains(target.SteamID.ToString()))
                {
                    BuilderData[player.Slot] = new Building.BuilderData { BlockType = Blocks.Models.Data.Platform.Title };
                    Building.PlayerHolds[target] = new Building.BuildData();
                }
            }
        }
        else
        {
            Instance.buildMode = false;
            BuilderData.Clear();
            Building.PlayerHolds.Clear();
        }

        string status = Instance.buildMode ? "Enabled" : "Disabled";
        char color = Instance.buildMode ? ChatColors.Green : ChatColors.Red;

        Utils.PrintToChatAll($"Build Mode: {color}{status} {ChatColors.Grey}by {ChatColors.LightPurple}{player.PlayerName}");
    }

    public static void ManageBuilder(CCSPlayerController? player, string input)
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (!Utils.HasPermission(player))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}You don't have permission to manage Builders");
            return;
        }

        var targetPlayer = Utilities.GetPlayers()
            .FirstOrDefault(target => target.SteamID.ToString() == input);

        if (string.IsNullOrEmpty(input) || targetPlayer == null)
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Player not found");
            return;
        }

        bool isBuilder = BuilderData.TryGetValue(targetPlayer.Slot, out var builderData);

        if (isBuilder)
            BuilderData.Remove(targetPlayer.Slot);

        else BuilderData[player.Slot] = new Building.BuilderData { BlockType = Blocks.Models.Data.Platform.Title };

        var action = isBuilder ? "removed" : "granted";
        var color = isBuilder ? ChatColors.Red : ChatColors.Green;

        Utils.PrintToChat(targetPlayer, $"{ChatColors.LightPurple}{player.PlayerName} {color}{action} your access to Build");
        Utils.PrintToChat(player, $"{color}You {action} {ChatColors.LightPurple}{targetPlayer.PlayerName} {color}access to Build");

        var builders = Files.Builders.steamids;
        string steamId = targetPlayer.SteamID.ToString();

        if (isBuilder && builders.Contains(steamId))
            builders.Remove(steamId);

        else
        {
            if (!builders.Contains(steamId))
                builders.Add(steamId);
        }

        Files.Builders.Save(builders);
    }

    public static void BuildMenu(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (!Utils.BuildMode(player))
            return;

        Menu.Open(player, "Block Maker");
    }

    public static void BlockType(CCSPlayerController? player, string selectType)
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (string.IsNullOrEmpty(selectType))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}No block type specified");
            return;
        }

        if (string.Equals("Teleport", selectType, StringComparison.OrdinalIgnoreCase))
        {
            BuilderData[player.Slot].BlockType = "Teleport";
            Utils.PrintToChat(player, $"Selected Type: {ChatColors.White}Teleport");
            return;
        }

        var blockModels = Blocks.Models.Data;
        foreach (var model in blockModels.GetAllBlocks())
        {
            if (string.Equals(model.Title, selectType, StringComparison.OrdinalIgnoreCase))
            {
                BuilderData[player.Slot].BlockType = model.Title;
                Utils.PrintToChat(player, $"Selected Type: {ChatColors.White}{model.Title}");
                return;
            }
        }

        Utils.PrintToChat(player, $"{ChatColors.Red}Could not find {ChatColors.White}{selectType} {ChatColors.Red}in block types");
    }

    public static void BlockColor(CCSPlayerController? player, string selectColor = "None")
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (string.IsNullOrEmpty(selectColor))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}No color specified");
            return;
        }

        foreach (var color in Utils.ColorMapping.Keys)
        {
            if (string.Equals(color, selectColor, StringComparison.OrdinalIgnoreCase))
            {
                BuilderData[player.Slot].BlockColor = color;
                Utils.PrintToChat(player, $"Selected Color: {ChatColors.White}{color}");

                Blocks.RenderColor(player);

                return;
            }
        }

        Utils.PrintToChat(player, $"{ChatColors.Red}Could not find a matching color");
    }

    public static void CreateBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Create(player);
    }

    public static void DeleteBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Delete(player);
    }

    public static void RotateBlock(CCSPlayerController? player, string rotation)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Position(player, rotation, true);
    }

    public static void PositionBlock(CCSPlayerController? player, string position)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Position(player, position, false);
    }

    public static void SaveBlocks(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (Utils.GetPlacedBlocksCount() <= 0)
        {
            Utils.PrintToChatAll($"{ChatColors.Red}No blocks to save");
            return;
        }

        Files.EntitiesData.Save();
    }

    public static void Snapping(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        ToggleCommand(player, ref BuilderData[player.Slot].Snapping, "Block Snapping");
    }

    public static void Grid(CCSPlayerController? player, string grid)
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (string.IsNullOrEmpty(grid))
        {
            ToggleCommand(player, ref BuilderData[player.Slot].Grid, "Block Grid");
            return;
        }

        BuilderData[player.Slot].GridValue = float.Parse(grid);

        Utils.PrintToChat(player, $"Selected Grid: {ChatColors.White}{grid} Units");
    }

    public static void Noclip(CCSPlayerController? player)
    {
        if (player == null || player.NotValid())
            return;

        if (!Utils.BuildMode(player))
            return;

        ToggleCommand(player, ref BuilderData[player.Slot].Noclip, "Noclip");

        if (BuilderData[player.Slot].Noclip)
        {
            player.Pawn.Value!.MoveType = MoveType_t.MOVETYPE_NOCLIP;
            Schema.SetSchemaValue(player.Pawn.Value!.Handle, "CBaseEntity", "m_nActualMoveType", 8); // noclip
            Utilities.SetStateChanged(player.Pawn.Value!, "CBaseEntity", "m_MoveType");
        }

        else if (!BuilderData[player.Slot].Noclip)
        {
            player.Pawn.Value!.MoveType = MoveType_t.MOVETYPE_WALK;
            Schema.SetSchemaValue(player!.Pawn.Value!.Handle, "CBaseEntity", "m_nActualMoveType", 2); // walk
            Utilities.SetStateChanged(player!.Pawn.Value!, "CBaseEntity", "m_MoveType");
        }
    }

    public static void Godmode(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        ToggleCommand(player, ref BuilderData[player.Slot].Godmode, "Godmode");

        if (BuilderData[player.Slot].Godmode)
            player.Pawn()!.TakesDamage = false;

        else player.Pawn()!.TakesDamage = true;
    }

    public static void TestBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Test(player);
    }

    public static void ClearBlocks(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Delete(player, true);

        Utils.PlaySoundAll(config.Sounds.Building.Delete);
        Utils.PrintToChatAll($"{ChatColors.Red}Blocks cleared by {ChatColors.LightPurple}{player.PlayerName}");
    }

    public static void ConvertBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Convert(player);
    }

    public static void CopyBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Copy(player);
    }

    public static void LockBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Lock(player);
    }

    public static void LockAll(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.LockAll(player);
    }

    public static void TransparencyBlock(CCSPlayerController? player, string transparency = "100%")
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.Transparency(player, transparency);
    }

    public static void EffectBlock(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;
        
        Blocks.ChangeEffect(player);
    }

    public static void TeamBlock(CCSPlayerController? player, string team = "Both")
    {
        if (player == null || !AllowedCommand(player))
            return;

        var entity = player.GetBlockAim();

        if (entity == null || entity.Entity == null || string.IsNullOrEmpty(entity.Entity.Name))
            return;

        if (Blocks.Entities.TryGetValue(entity, out var block))
        {
            if (Utils.BlockLocked(player, block))
                return;

            Blocks.Entities[entity].Team = team;
            Utils.PrintToChat(player, $"Changed block team to {ChatColors.White}{team}");
        }
    }

    public static void Pole(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        ToggleCommand(player, ref BuilderData[player.Slot].BlockPole, "Pole");
    }

    public static void Properties(CCSPlayerController? player, string type, string input)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Blocks.ChangeProperties(player, type, input);
    }

    public static void LightSettings(CCSPlayerController? player, string type, string input)
    {
        if (player == null || !AllowedCommand(player))
            return;

        Lights.Settings(player, type, input);
    }

    public static void ResetProperties(CCSPlayerController? player)
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (!Utils.HasPermission(player))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}You don't have permission to reset properties");
            return;
        }

        foreach (var block in Blocks.Entities.Values)
        {
            if (Blocks.Properties.BlockProperties.TryGetValue(block.Type.Split('.')[0], out var defaultProperties))
            {
                block.Properties = new Blocks.Property
                {
                    Cooldown = defaultProperties.Cooldown,
                    Value = defaultProperties.Value,
                    Duration = defaultProperties.Duration,
                    OnTop = defaultProperties.OnTop,
                    Locked = defaultProperties.Locked,
                    Builder = block.Properties.Builder,
                };
            }
            else Utils.PrintToChatAll($"{ChatColors.Red}Failed to find {ChatColors.White}{block.Type} {ChatColors.Red}default properties");
        }
        Utils.PrintToChatAll($"{ChatColors.Red}All placed blocks properties have been reset!");
    }

    // SafeZone Commands
    public static void CreateSafeZone(CCSPlayerController? player, string input = "")
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (!Utils.BuildMode(player))
            return;

        var trimmedName = input.Trim();
        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Usage: {ChatColors.White}!safezone <name>");
            Utils.PrintToChat(player, $"{ChatColors.Grey}First use sets position 1, second use sets position 2 and creates the zone");
            return;
        }

        // Validate name length
        if (trimmedName.Length > 64)
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Zone name too long (max 64 characters)");
            return;
        }

        var pawn = player.Pawn();
        if (pawn == null || pawn.AbsOrigin == null)
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Could not get your position");
            return;
        }

        var currentPos = new Vector_t(pawn.AbsOrigin!.X, pawn.AbsOrigin.Y, pawn.AbsOrigin.Z);

        // Validate position
        if (float.IsNaN(currentPos.X) || float.IsNaN(currentPos.Y) || float.IsNaN(currentPos.Z) ||
            float.IsInfinity(currentPos.X) || float.IsInfinity(currentPos.Y) || float.IsInfinity(currentPos.Z))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Invalid position detected");
            return;
        }

        // Check if player has a pending position
        if (SafeZone.PendingPositions.TryGetValue(player, out var pos1) && pos1 != null)
        {
            // Second position - create zone
            var pos2 = currentPos;

            try
            {
                // Use config defaults
                var zoneId = SafeZone.CreateZone(trimmedName, pos1.Value, pos2, player);
                var zone = SafeZone.GetZone(zoneId);

                if (zone != null)
                {
                    zone.Godmode = config.Settings.SafeZone.DefaultGodmode;
                    zone.Healing = config.Settings.SafeZone.DefaultHealing;
                    zone.HealingAmount = Math.Max(0.1f, config.Settings.SafeZone.DefaultHealingAmount);
                    zone.HealingInterval = Math.Max(0.1f, config.Settings.SafeZone.DefaultHealingInterval);
                    zone.Notify = config.Settings.SafeZone.DefaultNotify;
                    zone.BlockDamageToOutside = config.Settings.SafeZone.DefaultBlockDamageToOutside;
                }

                SafeZone.PendingPositions.Remove(player);

                Utils.PrintToChat(player, $"{ChatColors.Green}SafeZone '{ChatColors.White}{trimmedName}{ChatColors.Green}' created! (ID: {ChatColors.White}{zoneId}{ChatColors.Green})");
                Files.SafeZoneData.Save();
            }
            catch (ArgumentException ex)
            {
                Utils.PrintToChat(player, $"{ChatColors.Red}{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Utils.PrintToChat(player, $"{ChatColors.Red}{ex.Message}");
            }
            catch (Exception ex)
            {
                Utils.PrintToChat(player, $"{ChatColors.Red}Failed to create SafeZone: {ex.Message}");
                Utils.Log($"Error creating SafeZone: {ex.Message}");
            }
        }
        else
        {
            // First position - store it
            SafeZone.PendingPositions[player] = currentPos;
            Utils.PrintToChat(player, $"{ChatColors.Green}Position 1 set. Use the command again to set position 2 and create zone '{ChatColors.White}{trimmedName}{ChatColors.Green}'");
        }
    }

    public static void ListSafeZones(CCSPlayerController? player, string input = "")
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (!Utils.BuildMode(player))
            return;

        var zoneIdOrName = input.Trim();
        if (SafeZone.Zones.Count == 0)
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}No SafeZones found on this map");
            return;
        }

        if (!string.IsNullOrEmpty(zoneIdOrName))
        {
            // Show specific zone details
            SafeZone.ZoneData? zone = null;

            // Try to parse as ID
            if (int.TryParse(zoneIdOrName, out int id))
            {
                zone = SafeZone.GetZone(id);
            }
            else
            {
                // Try to find by name
                zone = SafeZone.GetZoneByName(zoneIdOrName);
            }

            if (zone == null)
            {
                Utils.PrintToChat(player, $"{ChatColors.Red}SafeZone not found: {ChatColors.White}{zoneIdOrName}");
                return;
            }

            // Show detailed info
            Utils.PrintToChat(player, $"{ChatColors.Green}=== SafeZone Details ===");
            Utils.PrintToChat(player, $"{ChatColors.Grey}ID: {ChatColors.White}{zone.Id}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Name: {ChatColors.White}{zone.Name}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Creator: {ChatColors.White}{zone.Creator}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Created: {ChatColors.White}{zone.CreatedAt:yyyy-MM-dd HH:mm}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Position 1: {ChatColors.White}({zone.MinPosition.X:F1}, {zone.MinPosition.Y:F1}, {zone.MinPosition.Z:F1})");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Position 2: {ChatColors.White}({zone.MaxPosition.X:F1}, {zone.MaxPosition.Y:F1}, {zone.MaxPosition.Z:F1})");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Godmode: {(zone.Godmode ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Healing: {(zone.Healing ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")} {(zone.Healing ? $"(+{zone.HealingAmount} HP every {zone.HealingInterval}s)" : "")}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Notify: {(zone.Notify ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")}");
            Utils.PrintToChat(player, $"{ChatColors.Grey}Block Damage to Outside: {(zone.BlockDamageToOutside ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")}");
        }
        else
        {
            // List all zones
            Utils.PrintToChat(player, $"{ChatColors.Green}=== SafeZones ({ChatColors.White}{SafeZone.Zones.Count}{ChatColors.Green}) ===");
            foreach (var zone in SafeZone.Zones.Values.OrderBy(z => z.Id))
            {
                Utils.PrintToChat(player, $"{ChatColors.Grey}ID {ChatColors.White}{zone.Id}{ChatColors.Grey}: {ChatColors.White}{zone.Name} {ChatColors.Grey}({zone.Creator})");
                Utils.PrintToChat(player, $"{ChatColors.Grey}  Godmode: {(zone.Godmode ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")} | Healing: {(zone.Healing ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")} | Notify: {(zone.Notify ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")} | Block Damage: {(zone.BlockDamageToOutside ? ChatColors.Green + "ON" : ChatColors.Red + "OFF")}");
            }
            Utils.PrintToChat(player, $"{ChatColors.Grey}Use {ChatColors.White}!listzone <id/name>{ChatColors.Grey} for details");
        }
    }

    public static void DeleteSafeZone(CCSPlayerController? player, string input = "")
    {
        if (player == null || !AllowedCommand(player))
            return;

        if (!Utils.BuildMode(player))
            return;

        var zoneIdOrName = input.Trim();
        if (string.IsNullOrEmpty(zoneIdOrName))
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Usage: {ChatColors.White}!deletezone <id/name>");
            return;
        }

        SafeZone.ZoneData? zone = null;

        // Try to parse as ID
        if (int.TryParse(zoneIdOrName, out int id))
        {
            zone = SafeZone.GetZone(id);
        }
        else
        {
            // Try to find by name
            zone = SafeZone.GetZoneByName(zoneIdOrName);
        }

        if (zone == null)
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}SafeZone not found: {ChatColors.White}{zoneIdOrName}");
            return;
        }

        if (SafeZone.DeleteZone(zone.Id))
        {
            Utils.PrintToChat(player, $"{ChatColors.Green}SafeZone '{ChatColors.White}{zone.Name}{ChatColors.Green}' (ID: {ChatColors.White}{zone.Id}{ChatColors.Green}) deleted");
            Files.SafeZoneData.Save();
        }
        else
        {
            Utils.PrintToChat(player, $"{ChatColors.Red}Failed to delete SafeZone");
        }
    }
}