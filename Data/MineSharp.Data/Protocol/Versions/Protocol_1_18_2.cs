namespace MineSharp.Data.Protocol.Versions;

internal class Protocol_1_18_2 : ProtocolVersion
{
    public override Dictionary<PacketType, int> PacketIds { get; } = new Dictionary<PacketType, int>()
    {
        { PacketType.SB_Handshake_SetProtocol, 0x00 },
        { PacketType.SB_Handshake_LegacyServerListPing, 0xfe },
        { PacketType.CB_Status_ServerInfo, 0x00 },
        { PacketType.CB_Status_Ping, 0x01 },
        { PacketType.SB_Status_PingStart, 0x00 },
        { PacketType.SB_Status_Ping, 0x01 },
        { PacketType.CB_Login_Disconnect, 0x00 },
        { PacketType.CB_Login_EncryptionBegin, 0x01 },
        { PacketType.CB_Login_Success, 0x02 },
        { PacketType.CB_Login_Compress, 0x03 },
        { PacketType.CB_Login_LoginPluginRequest, 0x04 },
        { PacketType.SB_Login_LoginStart, 0x00 },
        { PacketType.SB_Login_EncryptionBegin, 0x01 },
        { PacketType.SB_Login_LoginPluginResponse, 0x02 },
        { PacketType.CB_Play_SpawnEntity, 0x00 },
        { PacketType.CB_Play_SpawnEntityExperienceOrb, 0x01 },
        { PacketType.CB_Play_SpawnEntityLiving, 0x02 },
        { PacketType.CB_Play_SpawnEntityPainting, 0x03 },
        { PacketType.CB_Play_NamedEntitySpawn, 0x04 },
        { PacketType.CB_Play_SculkVibrationSignal, 0x05 },
        { PacketType.CB_Play_Animation, 0x06 },
        { PacketType.CB_Play_Statistics, 0x07 },
        { PacketType.CB_Play_AcknowledgePlayerDigging, 0x08 },
        { PacketType.CB_Play_BlockBreakAnimation, 0x09 },
        { PacketType.CB_Play_TileEntityData, 0x0a },
        { PacketType.CB_Play_BlockAction, 0x0b },
        { PacketType.CB_Play_BlockChange, 0x0c },
        { PacketType.CB_Play_BossBar, 0x0d },
        { PacketType.CB_Play_Difficulty, 0x0e },
        { PacketType.CB_Play_Chat, 0x0f },
        { PacketType.CB_Play_ClearTitles, 0x10 },
        { PacketType.CB_Play_TabComplete, 0x11 },
        { PacketType.CB_Play_DeclareCommands, 0x12 },
        { PacketType.CB_Play_CloseWindow, 0x13 },
        { PacketType.CB_Play_WindowItems, 0x14 },
        { PacketType.CB_Play_CraftProgressBar, 0x15 },
        { PacketType.CB_Play_SetSlot, 0x16 },
        { PacketType.CB_Play_SetCooldown, 0x17 },
        { PacketType.CB_Play_CustomPayload, 0x18 },
        { PacketType.CB_Play_NamedSoundEffect, 0x19 },
        { PacketType.CB_Play_KickDisconnect, 0x1a },
        { PacketType.CB_Play_EntityStatus, 0x1b },
        { PacketType.CB_Play_Explosion, 0x1c },
        { PacketType.CB_Play_UnloadChunk, 0x1d },
        { PacketType.CB_Play_GameStateChange, 0x1e },
        { PacketType.CB_Play_OpenHorseWindow, 0x1f },
        { PacketType.CB_Play_InitializeWorldBorder, 0x20 },
        { PacketType.CB_Play_KeepAlive, 0x21 },
        { PacketType.CB_Play_MapChunk, 0x22 },
        { PacketType.CB_Play_WorldEvent, 0x23 },
        { PacketType.CB_Play_WorldParticles, 0x24 },
        { PacketType.CB_Play_UpdateLight, 0x25 },
        { PacketType.CB_Play_Login, 0x26 },
        { PacketType.CB_Play_Map, 0x27 },
        { PacketType.CB_Play_TradeList, 0x28 },
        { PacketType.CB_Play_RelEntityMove, 0x29 },
        { PacketType.CB_Play_EntityMoveLook, 0x2a },
        { PacketType.CB_Play_EntityLook, 0x2b },
        { PacketType.CB_Play_VehicleMove, 0x2c },
        { PacketType.CB_Play_OpenBook, 0x2d },
        { PacketType.CB_Play_OpenWindow, 0x2e },
        { PacketType.CB_Play_OpenSignEntity, 0x2f },
        { PacketType.CB_Play_Ping, 0x30 },
        { PacketType.CB_Play_CraftRecipeResponse, 0x31 },
        { PacketType.CB_Play_Abilities, 0x32 },
        { PacketType.CB_Play_EndCombatEvent, 0x33 },
        { PacketType.CB_Play_EnterCombatEvent, 0x34 },
        { PacketType.CB_Play_DeathCombatEvent, 0x35 },
        { PacketType.CB_Play_PlayerInfo, 0x36 },
        { PacketType.CB_Play_FacePlayer, 0x37 },
        { PacketType.CB_Play_Position, 0x38 },
        { PacketType.CB_Play_UnlockRecipes, 0x39 },
        { PacketType.CB_Play_EntityDestroy, 0x3a },
        { PacketType.CB_Play_RemoveEntityEffect, 0x3b },
        { PacketType.CB_Play_ResourcePackSend, 0x3c },
        { PacketType.CB_Play_Respawn, 0x3d },
        { PacketType.CB_Play_EntityHeadRotation, 0x3e },
        { PacketType.CB_Play_MultiBlockChange, 0x3f },
        { PacketType.CB_Play_SelectAdvancementTab, 0x40 },
        { PacketType.CB_Play_ActionBar, 0x41 },
        { PacketType.CB_Play_WorldBorderCenter, 0x42 },
        { PacketType.CB_Play_WorldBorderLerpSize, 0x43 },
        { PacketType.CB_Play_WorldBorderSize, 0x44 },
        { PacketType.CB_Play_WorldBorderWarningDelay, 0x45 },
        { PacketType.CB_Play_WorldBorderWarningReach, 0x46 },
        { PacketType.CB_Play_Camera, 0x47 },
        { PacketType.CB_Play_HeldItemSlot, 0x48 },
        { PacketType.CB_Play_UpdateViewPosition, 0x49 },
        { PacketType.CB_Play_UpdateViewDistance, 0x4a },
        { PacketType.CB_Play_SpawnPosition, 0x4b },
        { PacketType.CB_Play_ScoreboardDisplayObjective, 0x4c },
        { PacketType.CB_Play_EntityMetadata, 0x4d },
        { PacketType.CB_Play_AttachEntity, 0x4e },
        { PacketType.CB_Play_EntityVelocity, 0x4f },
        { PacketType.CB_Play_EntityEquipment, 0x50 },
        { PacketType.CB_Play_Experience, 0x51 },
        { PacketType.CB_Play_UpdateHealth, 0x52 },
        { PacketType.CB_Play_ScoreboardObjective, 0x53 },
        { PacketType.CB_Play_SetPassengers, 0x54 },
        { PacketType.CB_Play_Teams, 0x55 },
        { PacketType.CB_Play_ScoreboardScore, 0x56 },
        { PacketType.CB_Play_SimulationDistance, 0x57 },
        { PacketType.CB_Play_SetTitleSubtitle, 0x58 },
        { PacketType.CB_Play_UpdateTime, 0x59 },
        { PacketType.CB_Play_SetTitleText, 0x5a },
        { PacketType.CB_Play_SetTitleTime, 0x5b },
        { PacketType.CB_Play_EntitySoundEffect, 0x5c },
        { PacketType.CB_Play_SoundEffect, 0x5d },
        { PacketType.CB_Play_StopSound, 0x5e },
        { PacketType.CB_Play_PlayerlistHeader, 0x5f },
        { PacketType.CB_Play_NbtQueryResponse, 0x60 },
        { PacketType.CB_Play_Collect, 0x61 },
        { PacketType.CB_Play_EntityTeleport, 0x62 },
        { PacketType.CB_Play_Advancements, 0x63 },
        { PacketType.CB_Play_EntityUpdateAttributes, 0x64 },
        { PacketType.CB_Play_EntityEffect, 0x65 },
        { PacketType.CB_Play_DeclareRecipes, 0x66 },
        { PacketType.CB_Play_Tags, 0x67 },
        { PacketType.SB_Play_TeleportConfirm, 0x00 },
        { PacketType.SB_Play_QueryBlockNbt, 0x01 },
        { PacketType.SB_Play_SetDifficulty, 0x02 },
        { PacketType.SB_Play_Chat, 0x03 },
        { PacketType.SB_Play_ClientCommand, 0x04 },
        { PacketType.SB_Play_Settings, 0x05 },
        { PacketType.SB_Play_TabComplete, 0x06 },
        { PacketType.SB_Play_EnchantItem, 0x07 },
        { PacketType.SB_Play_WindowClick, 0x08 },
        { PacketType.SB_Play_CloseWindow, 0x09 },
        { PacketType.SB_Play_CustomPayload, 0x0a },
        { PacketType.SB_Play_EditBook, 0x0b },
        { PacketType.SB_Play_QueryEntityNbt, 0x0c },
        { PacketType.SB_Play_UseEntity, 0x0d },
        { PacketType.SB_Play_GenerateStructure, 0x0e },
        { PacketType.SB_Play_KeepAlive, 0x0f },
        { PacketType.SB_Play_LockDifficulty, 0x10 },
        { PacketType.SB_Play_Position, 0x11 },
        { PacketType.SB_Play_PositionLook, 0x12 },
        { PacketType.SB_Play_Look, 0x13 },
        { PacketType.SB_Play_Flying, 0x14 },
        { PacketType.SB_Play_VehicleMove, 0x15 },
        { PacketType.SB_Play_SteerBoat, 0x16 },
        { PacketType.SB_Play_PickItem, 0x17 },
        { PacketType.SB_Play_CraftRecipeRequest, 0x18 },
        { PacketType.SB_Play_Abilities, 0x19 },
        { PacketType.SB_Play_BlockDig, 0x1a },
        { PacketType.SB_Play_EntityAction, 0x1b },
        { PacketType.SB_Play_SteerVehicle, 0x1c },
        { PacketType.SB_Play_Pong, 0x1d },
        { PacketType.SB_Play_RecipeBook, 0x1e },
        { PacketType.SB_Play_DisplayedRecipe, 0x1f },
        { PacketType.SB_Play_NameItem, 0x20 },
        { PacketType.SB_Play_ResourcePackReceive, 0x21 },
        { PacketType.SB_Play_AdvancementTab, 0x22 },
        { PacketType.SB_Play_SelectTrade, 0x23 },
        { PacketType.SB_Play_SetBeaconEffect, 0x24 },
        { PacketType.SB_Play_HeldItemSlot, 0x25 },
        { PacketType.SB_Play_UpdateCommandBlock, 0x26 },
        { PacketType.SB_Play_UpdateCommandBlockMinecart, 0x27 },
        { PacketType.SB_Play_SetCreativeSlot, 0x28 },
        { PacketType.SB_Play_UpdateJigsawBlock, 0x29 },
        { PacketType.SB_Play_UpdateStructureBlock, 0x2a },
        { PacketType.SB_Play_UpdateSign, 0x2b },
        { PacketType.SB_Play_ArmAnimation, 0x2c },
        { PacketType.SB_Play_Spectate, 0x2d },
        { PacketType.SB_Play_BlockPlace, 0x2e },
        { PacketType.SB_Play_UseItem, 0x2f },
    };
}