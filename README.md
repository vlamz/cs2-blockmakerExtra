# Creatify

**Advanced Building & SafeZone Plugin for Counter-Strike 2**

> [!NOTE]
> **Developed by vlamz**  
> Professional CS2 server enhancement tool with advanced building system and SafeZone module
>
> Block managing can be done within the building menu `/bm`
>
> Hold USE button to grab block, look around to move, left and right click to change distance
>
> Hold RELOAD button and move your mouse to rotate the block

<br>

| Video Showcase                                                                                               |
| ------------------------------------------------------------------------------------------------------------ |
| [![showcase](https://img.youtube.com/vi/IEcDrD1sUSc/hqdefault.jpg)](https://youtube.com/watch?v=IEcDrD1sUSc) |

<br>

## information:

### requirements

- [MetaMod](https://github.com/alliedmodders/metamod-source)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) v1.0.347
- [MultiAddonManager](https://github.com/Source2ZE/MultiAddonManager)
- [Block Maker Addon](https://steamcommunity.com/sharedfiles/filedetails/?id=3430295154)
- [CS2MenuManager](https://github.com/schwarper/CS2MenuManager)
- [ChaseMod](https://github.com/ipsvn/ChaseMod) (optional for gameplay)

<br>

> [!NOTE]
> **About Creatify:**
> - **Developer:** vlamz
> - **Version:** 0.3.0
> - **API:** CounterStrikeSharp v1.0.347
> - **License:** Proprietary
>
> **Credits & Acknowledgments:**
> - Built upon concepts from various CS2 building plugins
> - SafeZone module developed independently by vlamz
> - Inspired by community building tools

<img src="https://github.com/user-attachments/assets/53e486cc-8da4-45ab-bc6e-eb38145aba36" height="200px"> <br>

<br>

## example config

<details>
<summary>Creatify.json</summary>
  
```json
{
  "Settings": {
    "Prefix": "{purple}Creatify {grey}|",
    "MenuType": "CenterHtmlMenu",
    "Building": {
      "BuildMode": {
        "Enable": true,
        "Config": false
      },
      "AutoSave": {
        "Enable": true,
        "Timer": 300
      },
      "Grab": {
        "Render": true,
        "RenderColor": "255,255,255,128",
        "Beams": true,
        "BeamsColor": "255,255,255,255"
      }
    },
    "Blocks": {
      "DisableShadows": true,
      "CamouflageT": "characters/models/ctm_fbi/ctm_fbi.vmdl",
      "CamouflageCT": "characters/models/tm_leet/tm_leet_variantb.vmdl",
      "FireParticle": "particles/burning_fx/env_fire_medium.vpcf",
      "Sizes": [
        { "Title": "Small", "Size": 0.5 },
        { "Title": "Normal", "Size": 1 },
        { "Title": "Large", "Size": 2 },
        { "Title": "X-Large", "Size": 3 }
      ],
      "Effects": [
        { "Title": "Fire", "Particle": "particles/burning_fx/env_fire_small.vpcf" },
        { "Title": "Smoke", "Particle": "particles/burning_fx/smoke_gib_01.vpcf" },
        { "Title": "Money", "Particle": "particles/money_fx/moneybag_trail.vpcf" }
      ]
    },
    "Teleports": {
      "ForceAngles": false,
      "Velocity": 300,
      "Entry": {
        "Model": "models/blockmaker/teleport/model.vmdl",
        "Color": "0,255,0,255"
      },
      "Exit": {
        "Model": "models/blockmaker/teleport/model.vmdl",
        "Color": "255,0,0,255"
      }
    },
    "Lights": {
      "Model": "models/generic/interior_lamp_kit_01/ilk01_lamp_01_bulb.vmdl",
      "HideModel": true
    },
    "SafeZone": {
      "DefaultGodmode": true,
      "DefaultHealing": false,
      "DefaultHealingAmount": 1.0,
      "DefaultHealingInterval": 1.0,
      "DefaultNotify": true,
      "DefaultBlockDamageToOutside": true
    }
  },
  "Commands": {
    "Admin": {
      "Permission": ["@css/root"],
      "BuildMode": ["buildmode"],
      "ManageBuilder": ["builder", "builders"],
      "ResetProperties": ["resetproperties"]
    },
    "Building": {
      "BuildMenu": ["bm", "buildmenu"],
      "CreateBlock": ["create"],
      "DeleteBlock": ["delete"],
      "RotateBlock": ["rotate"],
      "PositionBlock": ["position"],
      "BlockType": ["type"],
      "BlockColor": ["color"],
      "CopyBlock": ["copy"],
      "ConvertBlock": ["convert"],
      "LockBlock": ["lock"],
      "LockAll": ["lockall"],
      "SaveBlocks": ["save"],
      "Snapping": ["snap"],
      "Grid": ["grid"],
      "Noclip": ["nc"],
      "Godmode": ["god"],
      "TestBlock": ["testblock"]
    },
    "SafeZone": {
      "Create": ["safezone", "sz"],
      "List": ["listzone", "listzones", "zones"],
      "Delete": ["deletezone", "removezone"]
    }
  },
  "Sounds": {
    "SoundEvents": "soundevents/creatify.vsndevts",
    "Blocks": {
      "Speed": "bm_speed",
      "Camouflage": "bm_camouflage",
      "Damage": "bm_damage",
      "Fire": "bm_fire",
      "Health": "bm_health",
      "Invincibility": "bm_invincibility",
      "Nuke": "bm_nuke",
      "Stealth": "bm_stealth",
      "Teleport": "bm_teleport"
    },
    "Building": {
      "Enabled": true,
      "Create": "bm_create",
      "Delete": "bm_delete",
      "Place": "bm_place",
      "Rotate": "bm_rotate",
      "Save": "bm_save"
    }
  }
}
```
</details>
<details> <summary>models.json</summary>
  
```json
{
  "Platform": {
    "Title": "Platform",
    "Block": "models/blockmaker/platform/block.vmdl",
    "Pole": "models/blockmaker/platform/pole.vmdl"
  },
  "Bhop": {
    "Title": "Bhop",
    "Block": "models/blockmaker/bhop/block.vmdl",
    "Pole": "models/blockmaker/bhop/pole.vmdl"
  },
  "Health": {
    "Title": "Health",
    "Block": "models/blockmaker/health/block.vmdl",
    "Pole": "models/blockmaker/health/pole.vmdl"
  },
  "Grenade": {
    "Title": "Grenade",
    "Block": "models/blockmaker/grenade/block.vmdl",
    "Pole": "models/blockmaker/grenade/pole.vmdl"
  },
  "Gravity": {
    "Title": "Gravity",
    "Block": "models/blockmaker/gravity/block.vmdl",
    "Pole": "models/blockmaker/gravity/pole.vmdl"
  },
  "Glass": {
    "Title": "Glass",
    "Block": "models/blockmaker/glass/block.vmdl",
    "Pole": "models/blockmaker/glass/pole.vmdl"
  },
  "Frost": {
    "Title": "Frost",
    "Block": "models/blockmaker/frost/block.vmdl",
    "Pole": "models/blockmaker/frost/pole.vmdl"
  },
  "Flash": {
    "Title": "Flash",
    "Block": "models/blockmaker/flash/block.vmdl",
    "Pole": "models/blockmaker/flash/pole.vmdl"
  },
  "Fire": {
    "Title": "Fire",
    "Block": "models/blockmaker/fire/block.vmdl",
    "Pole": "models/blockmaker/fire/pole.vmdl"
  },
  "Delay": {
    "Title": "Delay",
    "Block": "models/blockmaker/delay/block.vmdl",
    "Pole": "models/blockmaker/delay/pole.vmdl"
  },
  "Death": {
    "Title": "Death",
    "Block": "models/blockmaker/death/block.vmdl",
    "Pole": "models/blockmaker/death/pole.vmdl"
  },
  "Damage": {
    "Title": "Damage",
    "Block": "models/blockmaker/damage/block.vmdl",
    "Pole": "models/blockmaker/damage/pole.vmdl"
  },
  "Pistol": {
    "Title": "Pistol",
    "Block": "models/blockmaker/pistol/block.vmdl",
    "Pole": "models/blockmaker/pistol/pole.vmdl"
  },
  "Rifle": {
    "Title": "Rifle",
    "Block": "models/blockmaker/rifle/block.vmdl",
    "Pole": "models/blockmaker/rifle/pole.vmdl"
  },
  "Sniper": {
    "Title": "Sniper",
    "Block": "models/blockmaker/sniper/block.vmdl",
    "Pole": "models/blockmaker/sniper/pole.vmdl"
  },
  "SMG": {
    "Title": "SMG",
    "Block": "models/blockmaker/smg/block.vmdl",
    "Pole": "models/blockmaker/smg/pole.vmdl"
  },
  "ShotgunHeavy": {
    "Title": "Shotgun/Heavy",
    "Block": "models/blockmaker/heavy/block.vmdl",
    "Pole": "models/blockmaker/heavy/pole.vmdl"
  },
  "Stealth": {
    "Title": "Stealth",
    "Block": "models/blockmaker/stealth/block.vmdl",
    "Pole": "models/blockmaker/stealth/pole.vmdl"
  },
  "Speed": {
    "Title": "Speed",
    "Block": "models/blockmaker/speed/block.vmdl",
    "Pole": "models/blockmaker/speed/pole.vmdl"
  },
  "SpeedBoost": {
    "Title": "SpeedBoost",
    "Block": "models/blockmaker/speedboost/block.vmdl",
    "Pole": "models/blockmaker/speedboost/pole.vmdl"
  },
  "Slap": {
    "Title": "Slap",
    "Block": "models/blockmaker/slap/block.vmdl",
    "Pole": "models/blockmaker/slap/pole.vmdl"
  },
  "Random": {
    "Title": "Random",
    "Block": "models/blockmaker/random/block.vmdl",
    "Pole": "models/blockmaker/random/pole.vmdl"
  },
  "Nuke": {
    "Title": "Nuke",
    "Block": "models/blockmaker/nuke/block.vmdl",
    "Pole": "models/blockmaker/nuke/pole.vmdl"
  },
  "Invincibility": {
    "Title": "Invincibility",
    "Block": "models/blockmaker/invincibility/block.vmdl",
    "Pole": "models/blockmaker/invincibility/pole.vmdl"
  },
  "Ice": {
    "Title": "Ice",
    "Block": "models/blockmaker/ice/block.vmdl",
    "Pole": "models/blockmaker/ice/pole.vmdl"
  },
  "Camouflage": {
    "Title": "Camouflage",
    "Block": "models/blockmaker/camouflage/block.vmdl",
    "Pole": "models/blockmaker/camouflage/pole.vmdl"
  },
  "Trampoline": {
    "Title": "Trampoline",
    "Block": "models/blockmaker/trampoline/block.vmdl",
    "Pole": "models/blockmaker/trampoline/pole.vmdl"
  },
  "NoFallDmg": {
    "Title": "NoFallDmg",
    "Block": "models/blockmaker/nofall/block.vmdl",
    "Pole": "models/blockmaker/nofall/pole.vmdl"
  },
  "Honey": {
    "Title": "Honey",
    "Block": "models/blockmaker/honey/block.vmdl",
    "Pole": "models/blockmaker/honey/pole.vmdl"
  },
  "CustomBlocks": []
}
```
</details>
<details> <summary>default_properties.json</summary>
  
```json
{
  "Bhop": {
    "Cooldown": 1,
    "Value": 0,
    "Duration": 0.25,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Health": {
    "Cooldown": 0.75,
    "Value": 8,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Grenade": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Gravity": {
    "Cooldown": 5,
    "Value": 0.4,
    "Duration": 4,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Frost": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Flash": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Fire": {
    "Cooldown": 5,
    "Value": 8,
    "Duration": 5,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Delay": {
    "Cooldown": 1.5,
    "Value": 0,
    "Duration": 1,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Damage": {
    "Cooldown": 0.75,
    "Value": 8,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Stealth": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 7.5,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Speed": {
    "Cooldown": 60,
    "Value": 2,
    "Duration": 3,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "SpeedBoost": {
    "Cooldown": 0,
    "Value": 650,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Camouflage": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 10,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Slap": {
    "Cooldown": 0,
    "Value": 2,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Random": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Invincibility": {
    "Cooldown": 60,
    "Value": 0,
    "Duration": 5,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Trampoline": {
    "Cooldown": 0,
    "Value": 500,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Death": {
    "Cooldown": 0,
    "Value": 0,
    "Duration": 0,
    "OnTop": false,
    "Locked": false,
    "Builder": ""
  },
  "Honey": {
    "Cooldown": 0,
    "Value": 0.3,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Platform": {
    "Cooldown": 0,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "NoFallDmg": {
    "Cooldown": 0,
    "Value": 0,
    "Duration": 0,
    "OnTop": false,
    "Locked": false,
    "Builder": ""
  },
  "Ice": {
    "Cooldown": 0,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Nuke": {
    "Cooldown": 0,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Glass": {
    "Cooldown": 0,
    "Value": 0,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Pistol": {
    "Cooldown": 999,
    "Value": 1,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Rifle": {
    "Cooldown": 999,
    "Value": 1,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Sniper": {
    "Cooldown": 999,
    "Value": 1,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "Shotgun/Heavy": {
    "Cooldown": 999,
    "Value": 1,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  },
  "SMG": {
    "Cooldown": 999,
    "Value": 1,
    "Duration": 0,
    "OnTop": true,
    "Locked": false,
    "Builder": ""
  }
}
```
</details>

<br>

## Commands

### Admin Commands

| Command | Description | Example |
|---------|-------------|---------|
| `!buildmode` | Enable/disable build mode for all players | `!buildmode` |
| `!builder <steamid>` | Grant/remove builder access to a player | `!builder 76561197960287930` |
| `!resetproperties` | Reset all block properties to defaults | `!resetproperties` |

### Building Commands

| Command | Description | Example |
|---------|-------------|---------|
| `!bm` / `!buildmenu` | Open building menu | `!bm` |
| `!create` | Create a block at your crosshair position | `!create` |
| `!delete` | Delete the block you're looking at | `!delete` |
| `!type <name>` | Set block type | `!type Speed` |
| `!color <name>` | Set block color | `!color Red` |
| `!rotate <angle>` | Rotate block | `!rotate 90` |
| `!position <value>` | Move block position | `!position 8` |
| `!copy` | Copy block properties | `!copy` |
| `!convert` | Convert block type | `!convert` |
| `!lock` | Lock/unlock block | `!lock` |
| `!lockall` | Lock all blocks | `!lockall` |
| `!save` | Save all blocks to file | `!save` |
| `!snap` | Toggle block snapping | `!snap` |
| `!grid <value>` | Set grid size | `!grid 32` |
| `!nc` | Toggle noclip | `!nc` |
| `!god` | Toggle godmode | `!god` |
| `!testblock` | Test block functionality | `!testblock` |

### SafeZone Commands

| Command | Description | Example |
|---------|-------------|---------|
| `!safezone <name>` / `!sz <name>` | Create a safe zone (use twice: first sets corner 1, second sets corner 2) | `!safezone SpawnArea` |
| `!listzone` / `!listzones` / `!zones` | List all safe zones | `!listzone` |
| `!listzone <id>` / `!listzone <name>` | Show specific zone details | `!listzone 1` veya `!listzone SpawnArea` |
| `!deletezone <id>` / `!deletezone <name>` | Delete a safe zone | `!deletezone 1` veya `!deletezone SpawnArea` |
| `!removezone <id>` / `!removezone <name>` | Delete a safe zone (alias) | `!removezone SpawnArea` |
| `!zoneedit <id/name> <property> <value>` / `!editzone` / `!setzone` | Edit zone properties | `!zoneedit 1 godmode on` |

### Command Usage Examples

**Creating a SafeZone:**
```
1. Stand at the first corner of your desired zone
2. Type: !safezone SpawnArea
3. Move to the opposite corner
4. Type: !safezone SpawnArea again
5. Zone is created!
```

**Managing Blocks:**
```
!bm                    - Open menu
!type Speed            - Select Speed block type
!color Red             - Set color to red
!create                - Create block at crosshair
!delete                - Delete block you're looking at
!save                  - Save all blocks
```

**Listing SafeZones:**
```
!listzone              - List all zones with basic info
!listzone 1            - Show detailed info for zone ID 1
!listzone SpawnArea    - Show detailed info for zone named "SpawnArea"
```

**Editing SafeZone Properties:**
```
!zoneedit 1 godmode on          - Enable godmode for zone ID 1
!zoneedit SpawnArea healing off - Disable healing for zone named "SpawnArea"
!zoneedit 1 healingamount 5.0   - Set healing amount to 5.0 HP
!zoneedit 1 healinginterval 2.0 - Set healing interval to 2.0 seconds
!zoneedit 1 notify on           - Enable notifications for zone ID 1
!zoneedit 1 blockdamage off     - Disable damage blocking to outside

Properties: godmode, healing, healingamount, healinginterval, notify, blockdamage
Values: on/off/true/false (for bool) or <number> (for amount/interval)
```

<br>

## Changes & Updates

### Version 0.3.0 - Creatify Release (Latest)

**Rebranding:**
- ✅ Complete rebranding from BlockMaker to **Creatify**
- ✅ Plugin author updated to **vlamz**
- ✅ SafeZone module separated into `Creatify.Modules` namespace
- ✅ All entity names and references updated to use "creatify" prefix

**Major Features Added:**
- ✅ **SafeZone System** - Create safe zones with customizable properties
  - Two-position zone creation (first position sets corner 1, second sets corner 2)
  - Configurable godmode protection
  - Automatic healing with customizable amount and interval
  - Entry/exit notifications
  - Block damage to players outside the zone
  - Map-based zone storage and auto-loading
  - Zone listing and management commands

**Technical Improvements:**
- ✅ Updated to CounterStrikeSharp v1.0.347
- ✅ Fixed API compatibility issues
- ✅ Comprehensive memory leak prevention (timer cleanup on disconnect/death)
- ✅ Null reference protection throughout SafeZone system
- ✅ NaN/Infinity validation for all positions and values
- ✅ Integer overflow protection for zone IDs
- ✅ Duplicate zone name/ID prevention
- ✅ Exception handling and error recovery
- ✅ Performance optimizations (reduced GetPlayers() calls)
- ✅ Build mode godmode conflict resolution
- ✅ SafeZone module architecture with namespace separation
- ✅ Code organization improvements for maintainability

**Files Modified:**
- `src/Main.cs` - Rebranded to Creatify, version update (0.2.4 → 0.3.0), author set to vlamz, SafeZone hot reload support
- `src/Creatify.csproj` - Project renamed from BlockMaker.csproj
- `project.sln` - Solution updated for Creatify project
- `src/Config.cs` - Prefix updated to "Creatify", added SafeZone configuration settings (`Settings.SafeZone`)
- `src/Commands.cs` - Menu title updated, added SafeZone commands (create, list, delete, edit) with full validation
- `src/Events.cs` - Added SafeZone tick processing, damage blocking hook, player cleanup
- `src/Files.cs` - Added SafeZone save/load functionality (`SafeZoneData` class)
- `src/SafeZone.cs` - New SafeZone module in `Creatify.Modules` namespace, complete implementation (550+ lines)
- `src/Utils/Utils.cs` - Entity name prefixes updated to "creatify", added SafeZone cleanup to Clear() function
- `src/Blocks/Blocks.cs` - Entity names updated to use "creatify" prefix
- `src/Blocks/Models.cs` - Config file reference updated to "Creatify.json"
- `src/Teleports.cs` - Entity names updated to "creatify_Teleport_"
- `src/Lights.cs` - Entity names updated to "creatify_light" and "creatify_light_entity"
- `src/Blocks/Action.cs` - Entity name checks updated to "creatify" prefix

**What Changed:**
1. **SafeZone.cs** - Yeni dosya eklendi, zone yönetimi, pozisyon kontrolü, healing timer sistemi
2. **Events.cs** - OnTick'e SafeZone.OnTick() eklendi, OnTakeDamage hook'una SafeZone kontrolü eklendi, EventPlayerDeath'e SafeZone cleanup eklendi
3. **Commands.cs** - CreateSafeZone, ListSafeZones, DeleteSafeZone fonksiyonları eklendi
4. **Files.cs** - SafeZoneData.Save() ve Load() fonksiyonları eklendi, map bazlı JSON kayıt
5. **Config.cs** - Settings_SafeZone class'ı ve default değerler eklendi
6. **Main.cs** - Hot reload'da SafeZone yükleme eklendi

**Bug Fixes:**
- ✅ Fixed BuildMode function: BuilderData assignment now uses `target.Slot` instead of `player.Slot` when enabling build mode for eligible players
- ✅ Fixed ManageBuilder function: BuilderData assignment now uses `targetPlayer.Slot` instead of `player.Slot` when granting builder access
- ✅ Fixed SafeZone godmode: Players in zones with Godmode disabled now properly have their damage restored (TakesDamage set to true)
- ✅ Fixed SafeZone creation: Zone name from first command call is now preserved when setting second position (previously used second command's name)

### Version 0.2.4 → 0.3.0 Migration Notes

If you're upgrading from version 0.2.4:
- Make sure you have CounterStrikeSharp v1.0.347 or later installed
- No configuration changes are required (SafeZone uses default config values)
- All existing blocks and saves are compatible
- SafeZone data will be stored in `maps/<mapname>/safezones.json`
- New config section `Settings.SafeZone` added with default values
- SafeZone commands are available (see Commands section above for details)

<br>

**Creatify** - Professional CS2 Building & SafeZone Plugin  
**Developed by vlamz** | Version 0.3.0
